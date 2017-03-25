using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetworkPlayerManager : MonoBehaviour
{
    public Dictionary<int, NetworkPlayer> Players = new Dictionary<int, NetworkPlayer>();

    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private List<Vector3> PlayerPositions;
    [SerializeField] private Vector3 QueuePosition;
    [SerializeField] private Vector3 ShooterPosition;

    private bool[] PositionsBusy;
    private List<int> PlayerQueue;

    private void CreateShooter(short playerControllerId, NetworkConnection conn)
    {
        GameObject playerController = (GameObject)GameObject.Instantiate(PlayerPrefab);
        playerController.transform.parent = transform;
        NetworkPlayer player = playerController.GetComponent<NetworkPlayer>();
        player.transform.localPosition = ShooterPosition;
        player.IsShooter = true;
        player.name = "player_shooter";
        Players.Add(conn.hostId, player);
        NetworkServer.AddPlayerForConnection(conn, playerController, playerControllerId);
    }

    private void CreatePlayer(short playerControllerId, NetworkConnection conn)
    {
        if (conn.hostId == -1)
        {
            CreateShooter(playerControllerId, conn);
            return;
        }
        GameObject playerController = (GameObject)GameObject.Instantiate(PlayerPrefab);
        playerController.transform.parent = transform;
        NetworkPlayer player = playerController.GetComponent<NetworkPlayer>();
        if (!PlacePlayer(player))
        {
            playerController.transform.localPosition = QueuePosition;
            PlayerQueue.Add(conn.hostId); //new NetworkPlayerQueueItem(playerControllerId, conn));
        }

        Players.Add(conn.hostId, player);
        NetworkServer.AddPlayerForConnection(conn, playerController, playerControllerId);
    }

    private void DeletePlayer(int hostId, NetworkConnection conn)
    {
        if (!Players.ContainsKey(hostId))
        {
            Debug.LogError("User " + hostId + " not in room");
            return;
        }

        NetworkPlayer player = Players[hostId];
        if (player.PositionInRoom != -1)
            SetPositionState(player.PositionInRoom, false);
        
        
        Players.Remove(hostId);

        if (PlayerQueue.Count > 0 && player.PositionInRoom != -1)
        {
            PlacePlayer(Players[PlayerQueue[0]]);
            PlayerQueue.RemoveAt(0);
        }
    }

    private bool PlacePlayer(NetworkPlayer player)
    {
        int positionId = GetFreePosition();
        if (positionId == -1)
            return false;
        
        SetPositionState(positionId, true);
        player.transform.localPosition = PlayerPositions[positionId];
        player.name = "player_" + positionId;
        return true;
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
        NetworkingMainSingletone.Instance.NetworkEventManager.AddListener<int, NetworkConnection> (NetworkingEvents.ServerClientDisconnectd, DeletePlayer);
    }

    private void OnDestroy()
    {
        EventManager<NetworkingEvents> eventManager = NetworkingMainSingletone.Instance.NetworkEventManager;
        if (eventManager.HasListener<short, NetworkConnection>(NetworkingEvents.ServerClientConnected, CreatePlayer))
            eventManager.RemoveListener<short, NetworkConnection>(NetworkingEvents.ServerClientConnected, CreatePlayer);

        if (eventManager.HasListener<int, NetworkConnection> (NetworkingEvents.ServerClientDisconnectd, DeletePlayer))
            eventManager.RemoveListener<int, NetworkConnection> (NetworkingEvents.ServerClientDisconnectd, DeletePlayer);
    }

    /*private class NetworkPlayerQueueItem
    {
        public short PlayerControllerId { get; private set; }
        public NetworkConnection Connection { get; private set; }

        public NetworkPlayerQueueItem(short playerControllerId, NetworkConnection conn)
        {
            Connection = conn;
            PlayerControllerId = playerControllerId;
        }
    }*/
}

