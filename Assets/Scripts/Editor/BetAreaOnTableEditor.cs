using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

[CustomEditor(typeof(BetAreaOnTable))]
[CanEditMultipleObjects]
public class BetAreaOnTableEditor : Editor
{ 
    SerializedProperty BetName;
    int _choiceIndex = 0;

    void OnEnable()
    {
        BetName = serializedObject.FindProperty("BetName");
        _choiceIndex = Array.IndexOf(BetAreaOnTable.PossibleBetNames, BetName.stringValue);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        _choiceIndex = EditorGUILayout.Popup("BetName", _choiceIndex, BetAreaOnTable.PossibleBetNames);
        if (_choiceIndex < 0)
            _choiceIndex = 0;
        BetName.stringValue = BetAreaOnTable.PossibleBetNames[_choiceIndex];

        serializedObject.ApplyModifiedProperties();
    }
}

