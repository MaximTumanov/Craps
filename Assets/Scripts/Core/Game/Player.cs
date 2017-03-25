using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Balance;

    public List<Bet> Bets;

    public int Id = 0;
    public int CurrentBet = 100;

    [ContextMenu ("DoBet")]
    public void DoBet(int id)
    {
        Bets.Add(new Bet(id,CurrentBet));
    }

    public void KillBets()
    {
        Bets.Clear();
    }

}
