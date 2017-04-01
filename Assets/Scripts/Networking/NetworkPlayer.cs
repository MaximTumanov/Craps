using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkPlayer : NetworkBehaviour
{
    public System.Action<NetworkPlayer, int> ShowChipFly;
    public System.Action<NetworkPlayer, string, int> OnPutBet;
    public static NetworkPlayer CurrentPlayer;

    [System.NonSerialized] public NetworkCharacter Character;

    [System.NonSerialized] public int PlayerId = 0;
    [SyncVar (hook="PositionChanged")] public int PositionInRoom = -1;
    [System.NonSerialized] public bool IsShooter = false;
    public bool BetsReady = false;

    [SyncVar (hook="OnCharId")] public int CharId = -1;
    public long Coins = 1000;

    public override void OnStartLocalPlayer()
    {
        Debug.Log("OnStartLocalPlayer ");
        CurrentPlayer = this;
        PositionChanged(PositionInRoom);
        base.OnStartLocalPlayer();
        if (Character != null)
        {
            Character.gameObject.SetActive(false);
            Character.ApplyPositionToCamera();
        }
        else
        {
            Camera.main.transform.position = transform.position;
            Camera.main.transform.rotation = transform.rotation;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("OnStartClient CharId " + CharId);
        if (CharId != -1)
            OnCharId(CharId);
    }

    public void ApplyPosition(Vector3 localPosition, Quaternion localRotation)
    {
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
    }

    public void PutBet(string betId, int betValue)
    {
        if (betValue > Coins)
            return;

        Coins -= betValue;
        CmdPutBet(betId, betValue);
    }

    private void PositionChanged(int positionId)
    {
        if (!isLocalPlayer || IsShooter)
            return;

        if (PositionInRoom != -1)
            NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast(NetworkingEvents.PlacedToTable);
    }

    private void OnCharId(int charId)
    {
        if (Character != null)
            return;
        
        Debug.Log("OnCharId " + charId); 
        NetworkPlayerManager playerManager = FindObjectOfType<NetworkPlayerManager>();
        GameObject charObject = Instantiate<GameObject>(playerManager.PlayerCharacterPrefab[charId].gameObject);
        charObject.transform.SetParent(transform, false);
        Character = charObject.GetComponent<NetworkCharacter>();
        if (isLocalPlayer)
        {
            Debug.Log("OnStartLocalPlayer - OnCharId");
            Character.gameObject.SetActive(false);
            Character.ApplyPositionToCamera();
        }
    }


    [ClientRpc]
    public void RpcPayout(int coins)
    {
        if (!isLocalPlayer || IsShooter)
            return;
        Coins += coins;
    }

    public void BetsAvailable(List<int> betsId)
    {
        BetsReady = false;
        RpcBetsAvailable(betsId.ToArray());
    }

    [ClientRpc]
    public void RpcBetsAvailable(int[] betsId)
    {
        if (!isLocalPlayer || IsShooter)
            return;

        List<int> bets = new List<int>(); 
        for (int i = 0; i < betsId.Length; i++)
        {
            bets.Add(betsId[i]);
        }
        NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast<List<int>>(NetworkingEvents.ClientControlsEnabled, bets);
    }

    [ClientRpc]
    public void RpcNotAvailable()
    {
        if (!isLocalPlayer || IsShooter)
            return;

        NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast<List<int>>(NetworkingEvents.ClientControlsEnabled, new List<int>());
    }

    [Command]
    public void CmdBetsReady()
    {
        BetsReady = true;
    }

    [Command]
    private void CmdPutBet(string betId, int betValue)
    {
        if (OnPutBet != null)
            OnPutBet(this, betId, betValue);

        if (ShowChipFly != null)
        {
            ShowChipFly(this, 0);
            for (int i = 1; i < betValue / 50; i++)
            {
                ShowChipFly(this, i);
            }
        }
    }

}

