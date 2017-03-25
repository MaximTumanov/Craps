using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCoinController : NetworkBehaviour
{
    [System.NonSerialized] public int Index;
    [System.NonSerialized] public Vector3 target;
	
    void Update()
    {
        if (!isServer)
            return;
        
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 30f);
        if (Vector3.Distance(transform.position, target) < 1f)
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}

