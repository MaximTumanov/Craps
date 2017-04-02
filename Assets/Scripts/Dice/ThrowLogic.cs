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
    System.Action<int> ResultCallback;
    private Vector3[] ComperToThis;
    //side according to index 
    private int[] Sides = new int[] { 2, 5, 4, 3, 6, 1 };
    void Start()
    {
        //  Throw(Disepower);


        Parent = transform.parent.gameObject;
    }

    void OnEnable()
    {
        // initialize compearable array 
        //should be done only once 
        ComperToThis = new Vector3[]
        {
        transform.up,
        -transform.up,
        -transform.right,
        transform.right,
        transform.forward,
        -transform.forward
        };
        // return correct side of the dice 
        Debug.Log(GetDirection(transform, Sides) + " Side of this dice");
    }



    public void Throw(float powerOfThrow)
    {
        Dise.useGravity = true;
        Dise.GetComponent<BoxCollider>().enabled = true;
        Dise.AddForce(Vector3.forward * powerOfThrow);
        Dise.angularVelocity = new Vector3(Random.Range(0, 100f), Random.Range(0, 100f), Random.Range(0, 100f));
        StartCoroutine(DestroythisObje());
        //ResultCallback = resultCallbacl;
    }

    private int GetDirection(Transform dice, int[] sides)
    {
        float tempValue = 0;

        int sideIndex = 0;

        for (int i = 0; i < ComperToThis.Length; i++)
        {

            if (tempValue > Vector3.Dot(ComperToThis[i], Vector3.up))
            {
                tempValue = Vector3.Dot(ComperToThis[i], Vector3.up);

                sideIndex = i;

                Debug.Log(sides[sideIndex] + "  " + sideIndex);
            }
        }

        return sides[sideIndex];
    }
      


    IEnumerator DestroythisObje()
    {
        while (true)
        {
            if (Dise.IsSleeping())
            {
                Destroy(gameObject, .2f);
               Debug.Log( GetDirection(transform, Sides) + " Side of this dice");
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
