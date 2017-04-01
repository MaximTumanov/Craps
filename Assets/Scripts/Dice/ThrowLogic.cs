using UnityEngine;
using System.Collections;

public class ThrowLogic : MonoBehaviour
{

    [SerializeField]
    private Rigidbody Dise;
    public float Disepower;
    [SerializeField]
    private GameObject Parent;
    public static System.Action OntheEnd;
    public bool me;

    void Start()
    {
        //  Throw(Disepower);

        Parent = transform.parent.gameObject;
    }



    public void Throw(float powerOfThrow)
    {
        Dise.useGravity = true;
        Dise.GetComponent<BoxCollider>().enabled = true;
        Dise.AddForce(Vector3.forward * powerOfThrow);
        Dise.angularVelocity = new Vector3(Random.Range(0, 100f), Random.Range(0, 100f), Random.Range(0, 100f));
        StartCoroutine(DestroythisObje());
    }

    IEnumerator DestroythisObje()
    {
        while (true)
        {
            if (Dise.IsSleeping())
            {
                Destroy(gameObject, .2f);

                if (Parent != null && me)
                {
                    OntheEnd();
                    Destroy(Parent);

                }

                Debug.Log(transform.eulerAngles.normalized);
                break;
            }
            yield return null;
        }


    }


}
