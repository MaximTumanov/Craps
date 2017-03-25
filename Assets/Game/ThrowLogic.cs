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
        
    }

   
}
