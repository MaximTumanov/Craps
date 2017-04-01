using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableActionController : MonoBehaviour
{
    [SerializeField] private LayerMask RaycastMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ManageAction(Input.mousePosition);
    }

    private void ManageAction(Vector3 positionOnScreen)
    {
        Ray ray = Camera.main.ScreenPointToRay(positionOnScreen);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 1000, RaycastMask))
        {
            BetAreaOnTable hitArea = hit.collider.GetComponent<BetAreaOnTable>();
            if(hitArea)
            {
                //Add logic here
                Debug.Log(hitArea.BetName);
            }
        }
    }
}
