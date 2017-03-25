using UnityEngine;
using System.Collections;

public class ThrowLogic : MonoBehaviour {

    [SerializeField]
    private Rigidbody Dise; 
    public float Disepower;

    void Start()
    {        
        Throw(Disepower);
    }

    private void Throw(float powerOfThrow)
    {

        Dise.useGravity = true;

        Dise.AddForce(Vector3.forward * powerOfThrow);
        Dise.angularVelocity = new Vector3(Random.Range(0, 100f), Random.Range(0, 100f), Random.Range(0, 100f));
        StartCoroutine(DestroythisObje());

    }

    IEnumerator DestroythisObje()
    {
        while(true)
        {
            if(Dise.IsSleeping())
            {
                Destroy(gameObject, .3f);
                break;
            }
            yield return null;
        }
    }

   
}
