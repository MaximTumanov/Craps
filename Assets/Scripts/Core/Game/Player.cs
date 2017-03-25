using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public int Balance;
    public int PlayerPointer = -1;
    public List<Bet> Bets;

    public int Id = 0;
    public int CurrentBet = 100;

    [ContextMenu ("DoBet")]
    public void DoBet(string name)
    {
        Bets.Add(new Bet(name,CurrentBet));
    }

    public void KillBets()
    {
        Bets.Clear();
    }

}
