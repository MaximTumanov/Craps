using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkService : NetworkManager 
{

	public void Disconnect ()
	{
		StopHost ();

	}

	public bool IsConnected()
	{
        return IsClientConnected();
	}

	//Server is started
	public override void OnStartServer ()
	{
		base.OnStartServer ();
		NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast (NetworkingEvents.ConnectedAsServer);
	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
        if (NetworkingMainSingletone.Instance.HostMode)
        {
            //create host prefab
            return;
        }
//		base.OnServerAddPlayer (conn, playerControllerId);
        NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast<short, NetworkConnection> (NetworkingEvents.ServerClientConnected, playerControllerId, conn);
	}

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast<short, NetworkConnection> (NetworkingEvents.ServerClientDisconnectd, player.playerControllerId, conn);
    }

	public override void OnClientConnect (NetworkConnection conn)
	{
//		base.OnClientConnect (conn);
		if (NetworkingMainSingletone.Instance.HostMode)
			return; //you are not real client

		NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast (NetworkingEvents.ConnectedAsClient);
	}

	public override void OnClientDisconnect (NetworkConnection conn)
	{
		Debug.LogError ("NetworkService: OnClientDisconnect");
		base.OnClientDisconnect (conn);
		NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast (NetworkingEvents.Disconnected);
	}

}
