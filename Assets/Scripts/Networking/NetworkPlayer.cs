using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkPlayer : NetworkBehaviour
{
    private string CurrentBetText = "100";

    [System.NonSerialized] public int PositionInRoom = -1;
    [System.NonSerialized] public bool IsShooter = false;

    [SyncVar]
    public long Coins = 1000;
    private int PlayerId;

    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
        if (isLocalPlayer)
        {
            //            NetworkingMainSingletone.Instance.NetworkEventManager.Broadcast(NetworkingEvents.
        }
    }

    private void OnGUI()
    {
        if (!isLocalPlayer || IsShooter)
            return;

        float scale = Screen.width / 1000f;

        CurrentBetText = GUI.TextField(new Rect(10f * scale, 10f * scale, 100f * scale, 30f * scale), CurrentBetText);
        for (int i = 0; i < 5; i++)
        {
            if (GUI.Button(new Rect(10f * scale, (50f + i * 50f) * scale, 100f * scale, 50f * scale), "Bet " + i))
            {
                CmdPutBet(i, System.Convert.ToInt32(CurrentBetText));
            }
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
    }

    [ClientRpc]
    private void RpcNotAvailable()
    {
        if (!isLocalPlayer || IsShooter)
            return;
    }

    [Command]
    private void CmdPutBet(int betId, int betValue)
    {
        //Call pay table
    }

}

