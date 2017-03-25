using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkConnectScreen : MonoBehaviour
{
	[SerializeField] private InputField IpField;
	[SerializeField] private GameObject MenuContainer;
	[SerializeField] private GameObject ProgressContainer;
	[SerializeField] private float Timeout = 5f;

	private bool ConnectionInProgress = false;
	private float ConnectStartTime = 0f;

	public void ConnectAsServer()
	{
		NetworkingMainSingletone.Instance.SetConnectionMode (true);
        NetworkingMainSingletone.Instance.NetworkService.StartHost ();

		StartConnection ();
	}

	public void ConnectAsClient()
	{
		NetworkingMainSingletone.Instance.SetConnectionMode (false);
        NetworkingMainSingletone.Instance.NetworkService.networkAddress = IpField.text;
        NetworkingMainSingletone.Instance.NetworkService.StartClient ();
		StartConnection ();
	}

	private void Start()
	{
//        IpField.text = NetworkingMainSingletone.Instance.NetworkService.networkAddress;
		ShowConnectMenu ();
		NetworkingMainSingletone.Instance.NetworkEventManager.AddListener (NetworkingEvents.Disconnected, ShowConnectMenu);
	}

	private void OnDestroy()
	{
		if (NetworkingMainSingletone.Instance.NetworkEventManager.HasListener (NetworkingEvents.Disconnected, ShowConnectMenu))
			NetworkingMainSingletone.Instance.NetworkEventManager.RemoveListener (NetworkingEvents.Disconnected, ShowConnectMenu);
	}

	private IEnumerator TimerCycle ()
	{
        while (ConnectionInProgress && Time.time - ConnectStartTime < Timeout) 
		{
            if (NetworkingMainSingletone.Instance.NetworkService.IsConnected())
            {
                SceneManager.LoadScene (SceneNames.Game);
                yield break;
            }
            yield return new WaitForSeconds(0.2f);
		}
        ShowConnectMenu ();
	}

	private void StartConnection()
	{
		ConnectionInProgress = true;
		ConnectStartTime = Time.time;
		MenuContainer.SetActive (false);
		ProgressContainer.SetActive (true);
		StartCoroutine (TimerCycle());
	}

	private void ShowConnectMenu ()
	{
		ConnectionInProgress = false;
		MenuContainer.SetActive (true);
		ProgressContainer.SetActive (false);
	}
}

