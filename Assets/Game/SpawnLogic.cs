using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
public class SpawnLogic : MonoBehaviour {

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private VRInput VRi;

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
        
        tr.Disepower = ((Time.time - t) * 1000f) % 1200f;

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


}
