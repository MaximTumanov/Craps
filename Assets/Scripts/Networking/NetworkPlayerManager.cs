using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetworkPlayerManager : MonoBehaviour
{
    public Dictionary<int, NetworkPlayer> Players = new Dictionary<int, NetworkPlayer>();
    public List<NetworkCharacter> PlayerCharacterPrefab;

    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private List<Transform> PlayerPositions;
    [SerializeField] private Transform QueuePosition;
    [SerializeField] private Transform ShooterPosition;

    private bool[] PositionsBusy;
    private List<int> PlayerQueue = new List<int>();

    private void CreateShooter(short playerControllerId, NetworkConnection conn)
    {
        GameObject playerController = (GameObject)GameObject.Instantiate(PlayerPrefab);
        playerController.transform.parent = transform;
        NetworkPlayer player = playerController.GetComponent<NetworkPlayer>();
        player.transform.localPosition = ShooterPosition.localPosition;
        player.IsShooter = true;
        player.name = "player_shooter";
        Players.Add(conn.connectionId, player);
        NetworkServer.AddPlayerForConnection(conn, playerController, playerControllerId);
    }

    private void CreatePlayer(short playerControllerId, NetworkConnection conn)
    {
        Debug.Log("Add player " + conn.ToString());
        if (conn.hostId == -1)
        {
            CreateShooter(playerControllerId, conn);
            return;
        }
        GameObject playerController = GameObject.Instantiate<GameObject>(PlayerPrefab);
        playerController.transform.parent = transform;
        NetworkPlayer player = playerController.GetComponent<NetworkPlayer>();
        player.PlayerId = conn.connectionId;
        if (!PlacePlayer(player))
        {
            playerController.transform.localPosition = QueuePosition.localPosition;// + (Vector3)Random.insideUnitCircle * 100f;
            playerController.transform.localRotation = QueuePosition.localRotation;
            PlayerQueue.Add(conn.connectionId); //new NetworkPlayerQueueItem(playerControllerId, conn));
        }

        Players.Add(conn.connectionId, player);
        NetworkServer.AddPlayerForConnection(conn, playerController, playerControllerId);
    }

    private void DeletePlayer(int connectionId, NetworkConnection conn)
    {
        Debug.Log("Delete player " + conn.ToString() + " hid: " + connectionId);
        if (!Players.ContainsKey(connectionId))
        {
            Debug.LogError("User " + connectionId + " not in room");
            return;
        }

        NetworkPlayer player = Players[connectionId];
        if (player.PositionInRoom != -1)
            SetPositionState(player.PositionInRoom, false);
        
        
        Players.Remove(connectionId);

        if (PlayerQueue.Count > 0 && player.PositionInRoom != -1)
        {
            PlacePlayer(Players[PlayerQueue[0]], false);
            PlayerQueue.RemoveAt(0);
        }
    }

    private bool PlacePlayer(NetworkPlayer player, bool newPlayer = true)
    {
        int positionId = GetFreePosition();
        if (positionId == -1)
            return false;
        
        SetPositionState(positionId, true);
        player.PositionInRoom = positionId;
        player.transform.localPosition = PlayerPositions[positionId].localPosition;
        player.transform.localRotation = PlayerPositions[positionId].localRotation;
        player.ApplyPosition(PlayerPositions[positionId].localPosition, PlayerPositions[positionId].localRotation);
        player.name = "player_" + positionId;
        player.CharId = Random.Range(0, PlayerCharacterPrefab.Count);

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

