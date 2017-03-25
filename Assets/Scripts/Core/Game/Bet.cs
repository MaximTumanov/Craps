using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bet
{
    public string Name;
    public int Amount;

    public Bet(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }
}
