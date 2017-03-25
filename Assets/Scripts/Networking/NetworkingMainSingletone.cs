using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkingMainSingletone
{
	public static NetworkingMainSingletone Instance
	{
		get 
		{
            if (_instance == null)
            {
//                GameObject mainObject = new GameObject("NetworkingMainSingletone");
//                _instance = mainObject.AddComponent<NetworkingMainSingletone>();
                _instance = new NetworkingMainSingletone();
            }
			
			return _instance;
		}
	}
	private static NetworkingMainSingletone _instance;

	public EventManager<NetworkingEvents> NetworkEventManager;
	public NetworkService NetworkService;
	public bool HostMode { get; private set;}

    public NetworkingMainSingletone()
    {
		NetworkEventManager = new EventManager<NetworkingEvents> ();
        NetworkService = NetworkService.singleton as NetworkService;
	}

	public void SetConnectionMode(bool asHost)
	{
		HostMode = asHost;
	}
}

