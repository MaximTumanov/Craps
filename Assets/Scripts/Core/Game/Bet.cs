using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bet
{
    public int Id;
    public int Amount;

    public Bet(int id, int amount)
    {
        Id = id;
        Amount = amount;
    }
}
