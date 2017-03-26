using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bet
{
    public long Id;
    public string Name;
    public int Amount;

    public Bet(string name, int amount)
    {
        Id = (long)Random.Range(long.MinValue, long.MaxValue);
        Name = name;
        Amount = amount;
    }
}
