using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using VRStandardAssets.Utils;
using UnityEngine.UI;

public class SpawnLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GameObject Holder;

    [SerializeField]
    private GameObject SpawnPoint;

    [SerializeField]
    private VRInput VRi;
    [SerializeField]
    UnityEvent OnThrowDice;



   
    [SerializeField]
    ThrowLogic[] obj = new ThrowLogic[2];
    float t;

    private GameObject HolderOnScene;

    void Start()
    {
       InstHolder();
        ThrowLogic.OntheEnd = InstHolder;
        VRi.OnDown += VRi_OnDown;
        VRi.OnUp += VRi_OnUp;
    }

    private void InstHolder()
    {
        HolderOnScene = Instantiate(Holder, SpawnPoint.transform.position, Quaternion.identity) as GameObject;
       // HolderOnScene.GetComponent<OVRGrabbable>

    }


    private void VRi_OnUp()
    {

    }

    private void VRi_OnDown()
    {
        //  if (Input.GetMouseButtonDown(0))
        {
          //  obj[0].GetComponent<BoxCollider>().enabled = false;
           // obj[1].GetComponent<BoxCollider>().enabled = false;
         //   obj[0].Disepower = Mathf.Clamp((Time.time - t) * 3f, 0.1f, 1f) * obj[0].Disepower;
          //  obj[1].Disepower = Mathf.Clamp((Time.time - t) * 3f, 0.1f, 1f) * obj[1].Disepower;

            t = Time.time;
        }
    }

}