using System;
using UnityEngine;
using UnityEditor;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Object = UnityEngine.Object;

public class ReferenceFinder : EditorWindow
{
    private List<string> FoundObjects = new List<string>();
    [SerializeField]
    private List<Object> FoundGameObjects;
    private int CurrentProgress;
    private int FilesCount;

    private Dictionary<string, bool> AvailableTypes = new Dictionary<string, bool>()
    {
        {"*.prefab", true},
        {"*.mat", true},
        {"*.unity", true},
        {"*.asset", false}
    };
    private string Guid;

    private void SetUpSearch(string guid)
    {
        Guid = guid;
    }

    private void StartSearch(string guid)
    {
        if (string.IsNullOrEmpty(guid))
            return;

        Guid = guid;
        CurrentProgress = 0;
        FoundObjects = new List<string>();
        FoundGameObjects = new List<Object>();
        List<string> files = new List<string>();

        foreach (KeyValuePair<string, bool> availableType in AvailableTypes)
        {
            if (availableType.Value)
                files.AddRange(Directory.GetFiles("Assets/", availableType.Key, SearchOption.AllDirectories).ToList());
        }

        FilesCount = files.Count;
        List<List<string>> devidedPaths = Split(files, SystemInfo.processorCount - 1);

        foreach (List<string> paths in devidedPaths)
        {
            List<string> paths1 = paths;
            Thread thread = new Thread(() => FindReferences(paths1));
            thread.Start();
        }
    }


    public static List<List<T>> Split<T>(List<T> collection, int chunkCount)
    {
        int size = collection.Count() / chunkCount;
        List<List<T>> chunks = new List<List<T>>();
        for (var i = 0; i < chunkCount; i++)
            chunks.Add(collection.Skip(i * size).Take(i < chunkCount - 1 ? size : collection.Count - i * size).ToList());

        return chunks;
    }

    private void FindReferences(List<string> pahtList)
    {
        Thread thread = Thread.CurrentThread;
        foreach (string file in pahtList)
        {
            CurrentProgress++;
            using (StreamReader streamReader = new StreamReader(file))
            {
                if (streamReader.ReadToEnd().Contains(Guid))
                {
                    lock (FoundObjects)
                    {
                        FoundObjects.Add(file);
                    }
                }
            }
            GC.Collect();
        }
        thread.Join();
    }

    private Vector2 ScrollPosition;
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, 50));
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.TextArea(Guid);
        float progress = FilesCount == 0 ? 0f : CurrentProgress * 100f / FilesCount;
        GUILayout.Label("Progress: " + progress + "%");
        GUILayout.EndVertical();

        AvailableTypes["*.prefab"] = GUILayout.Toggle(AvailableTypes["*.prefab"], "Prefabs");
        AvailableTypes["*.mat"] = GUILayout.Toggle(AvailableTypes["*.mat"], "Materials");
        AvailableTypes["*.unity"] = GUILayout.Toggle(AvailableTypes["*.unity"], "Scenes");
        AvailableTypes["*.asset"] = GUILayout.Toggle(AvailableTypes["*.asset"], "Assets");

        if (GUILayout.Button("Search", GUILayout.MinWidth(150), GUILayout.MinHeight(50)))
            StartSearch(Guid);//ReferenceIndexer.Instance.IndexAllFiles();

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.Space(50);

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, true, true, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 50));
        lock (FoundObjects)
        {
            if (FoundObjects.Count > 0 && FoundGameObjects != null && FoundObjects.Count - FoundGameObjects.Count > 0)
            {
                for (int i = FoundGameObjects.Count; i < FoundObjects.Count; i++)
                {
                    Object newObject = AssetDatabase.LoadAssetAtPath<Object>(FoundObjects[i]);
                    if (FoundGameObjects.Contains(newObject)) continue;
                    FoundGameObjects.Add(newObject);
                }
            }
        }
        if (FoundGameObjects != null)
        {
            foreach (Object foundGameObject in FoundGameObjects)
            {
                if (foundGameObject == null) continue;
                EditorGUILayout.ObjectField(foundGameObject.name, foundGameObject, foundGameObject.GetType());
            }
        }

        GUILayout.EndScrollView();
    }

    [MenuItem("Assets/Find All References", false, 25)]
    public static void ShowSearch()
    {
        Object mainObject = Selection.objects.First();

        string path = AssetDatabase.GetAssetPath(mainObject);
        string guid = AssetDatabase.AssetPathToGUID(path);

        ReferenceFinder popup = GetWindow<ReferenceFinder>(true, "Reference Finder", true);
        popup.SetUpSearch(guid);
    }
}