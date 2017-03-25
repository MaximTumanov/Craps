using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using UnityEngine.UI;

public class SpawnLogic : MonoBehaviour {

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private VRInput VRi;

    

    [SerializeField]
    Text Text;

    ThrowLogic tr;

    GameObject obj;
    float t;

    void Start()
    {

        VRi.OnDown += VRi_OnDown;
        VRi.OnUp += VRi_OnUp;   
        

    }


 

    private void VRi_OnUp()
    {

        tr.Disepower = Mathf.Clamp((Time.time - t) * 3f, 0.1f, 1f) * tr.Disepower;

        obj.SetActive(true);
    }

    private void VRi_OnDown()
    {
      //  if (Input.GetMouseButtonDown(0))
        {
            obj = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;

            tr = obj.GetComponent<ThrowLogic>();

            t = Time.time;
        }
    }

    void Update()
    {
   


         if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            Text.text = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch).ToString() + ", " +
                 OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch).ToString() + ", " +
                  OVRInput.GetLocalControllerAngularAcceleration(OVRInput.Controller.RTouch).ToString() + ", " +
                   OVRInput.GetActiveController().ToString();


        }

         

    }


}
