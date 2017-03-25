using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NetworkReturnToLobby : MonoBehaviour
{
    private void Awake()
    {
        if (NetworkingMainSingletone.Instance == null)
            SceneManager.LoadScene (SceneNames.ServerConnect);
    }
	
	public void ReturnToLobby()
	{
		NetworkingMainSingletone.Instance.NetworkService.Disconnect ();
	}

	/*private void Awake()
	{
		NetworkingMainSingletone.Instance.NetworkEventManager.AddListener (NetworkingEvents.Disconnected, OnDisconnected);
	}

	private void OnDestroy()
	{
		if (NetworkingMainSingletone.Instance.NetworkEventManager.HasListener (NetworkingEvents.Disconnected, OnDisconnected))
			NetworkingMainSingletone.Instance.NetworkEventManager.RemoveListener (NetworkingEvents.Disconnected, OnDisconnected);
	}

	private void OnDisconnected()
	{
//		SceneManager.LoadScene (SceneNames.ServerConnect);
	}*/
}

