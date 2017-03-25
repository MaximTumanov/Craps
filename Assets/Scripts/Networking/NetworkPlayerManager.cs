using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetworkPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private List<Vector3> PlayerPositions;
    [SerializeField] private List<NetworkPlayerQueueItem> PlayerQueue;
    [SerializeField] private Vector3 QueuePosition;
    private bool[] PositionsBusy;

    public Dictionary<short, NetworkPlayer> Players = new Dictionary<short, NetworkPlayer>();

    private void CreatePlayer(short playerControllerId, NetworkConnection conn)
    {
        NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast<short> (NetworkingEvents.ServerClientConnected, playerControllerId);

        // Create the player "controller" object which has the client authority
        GameObject playerController = (GameObject)GameObject.Instantiate(PlayerPrefab);
        int positionId = GetFreePosition();
        if (positionId != -1)
        {
            SetPositionState(positionId, true);
            playerController.transform.localPosition = PlayerPositions[positionId];
        }
        else
        {
            playerController.transform.localPosition = QueuePosition;
            PlayerQueue.Add(new NetworkPlayerQueueItem(playerControllerId, conn));
        }

        NetworkServer.AddPlayerForConnection(conn, playerController, playerControllerId);

//        GameObject playerCharacter = (GameObject)GameObject.Instantiate(characterPrefab);
//        NetworkServer.Spawn(playerCharacter);
    }

    private void DeletePlayer(short playerControllerId, NetworkConnection conn)
    {
        if (!Players.ContainsKey(playerControllerId))
        {
            Debug.LogError("User " + playerControllerId + " not in room");
            return;
        }

        NetworkPlayer player = Players[playerControllerId];
        if (player.PositionInRoom != -1)
            SetPositionState(player.PositionInRoom, false);
        
        
        Players.Remove(playerControllerId);

        if (PlayerQueue.Count > 0)
        {
            int positionId = GetFreePosition();
            if (positionId == -1)
                return;

            SetPositionState(positionId, true);
            player.transform.localPosition = PlayerPositions[positionId];
        }
    }

    private void SetPositionState(int index, bool busy)
    {
        PositionsBusy[index] = busy;
    }

    private int GetFreePosition()
    {
        for (int i = 0; i < PositionsBusy.Length; i++)
        {
            if (PositionsBusy[i])
                continue;

            return i;
        }
        return -1;
    }

    private void Awake()
    {
        PositionsBusy = new bool[PlayerPositions.Count];
        NetworkingMainSingletone.Instance.NetworkEventManager.AddListener<short, NetworkConnection> (NetworkingEvents.ServerClientConnected, CreatePlayer);
        NetworkingMainSingletone.Instance.NetworkEventManager.AddListener<short, NetworkConnection> (NetworkingEvents.ServerClientDisconnectd, DeletePlayer);
    }

    private void Destroy()
    {
        EventManager<NetworkingEvents> eventManager = NetworkingMainSingletone.Instance.NetworkEventManager;
        if (eventManager.HasListener<short, NetworkConnection>(NetworkingEvents.ServerClientConnected, CreatePlayer))
            eventManager.RemoveListener<short, NetworkConnection>(NetworkingEvents.ServerClientConnected, CreatePlayer);

        if (eventManager.HasListener<short, NetworkConnection> (NetworkingEvents.ServerClientDisconnectd, CreatePlayer))
            eventManager.RemoveListener<short, NetworkConnection> (NetworkingEvents.ServerClientDisconnectd, CreatePlayer);
    }

    private class NetworkPlayerQueueItem
    {
        public short PlayerControllerId { get; private set; }
        public NetworkConnection Connection { get; private set; }

        public NetworkPlayerQueueItem(short playerControllerId, NetworkConnection conn)
        {
            Connection = conn;
            PlayerControllerId = playerControllerId;
        }
    }
}

