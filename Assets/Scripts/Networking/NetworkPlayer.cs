﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer CurrentPlayer;

    [System.NonSerialized] public NetworkCharacter Character;

    [System.NonSerialized] public int PlayerId = 0;
    [SyncVar (hook="PositionChanged")] public int PositionInRoom = -1;
    [System.NonSerialized] public bool IsShooter = false;

    [SyncVar (hook="OnCharId")] public int CharId = -1;
    [SyncVar] public long Coins = 1000;

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

    public void PutBet(int betId, int betValue)
    {
        if (betValue > Coins)
            return;

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
    private void RpcPayout(int coins)
    {
        if (!isLocalPlayer || IsShooter)
            return;
        Coins += coins;
    }

    [ClientRpc]
    private void RpcBetsAvailable(List<int> betsId)
    {
        if (!isLocalPlayer || IsShooter)
            return;

        NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast<List<int>>(NetworkingEvents.ClientControlsEnabled, betsId);
    }

    [ClientRpc]
    private void RpcNotAvailable()
    {
        if (!isLocalPlayer || IsShooter)
            return;

        NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast<List<int>>(NetworkingEvents.ClientControlsEnabled, new List<int>());
    }

    [Command]
    private void CmdPutBet(int betId, int betValue)
    {
        Coins -= betValue;
        //Call pay table

        //Spawn chip to fly
    }

}

