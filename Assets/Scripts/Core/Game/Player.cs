using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public int Id;
    public int Balance;
    public List<Bet> Bets;
    
    public Player(int id)
    {
        Id = id;
        Bets = new List<Bet>();
    }

    [ContextMenu ("DoBet")]
    public void DoBet(Bet bet)
    {
        Bets.Add(bet);
    }

    public void Remove(Bet bet)
    {
        Bets.Remove(bet);
    }

    public void KillBets()
    {
        Bets.Clear();
    }

}
