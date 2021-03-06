﻿using UnityEngine;
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
        Log("OnServerAddPlayer: " + conn.connectionId);
//		base.OnServerAddPlayer (conn, playerControllerId);
        NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast<short, NetworkConnection> (NetworkingEvents.ServerClientConnected, playerControllerId, conn);
	}
        
	public override void OnClientConnect (NetworkConnection conn)
	{
        Log("OnClientConnect");
		base.OnClientConnect (conn);
		if (NetworkingMainSingletone.Instance.HostMode)
			return; //you are not real client

		NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast (NetworkingEvents.ConnectedAsClient);
	}

	public override void OnClientDisconnect (NetworkConnection conn)
	{
        Log ("OnClientDisconnect");
		base.OnClientDisconnect (conn);
		NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast (NetworkingEvents.Disconnected);
	}

    public override void OnServerConnect(NetworkConnection conn)
    {
        Log ("OnServerConnect");
        base.OnServerConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Log ("OnServerDisconnect");
        base.OnServerDisconnect(conn);
        NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast<int, NetworkConnection> (NetworkingEvents.ServerClientDisconnectd, conn.connectionId, conn);
    }

    private void Log(string message)
    {
        Debug.Log("NetworkService: " + message);
    }
}
