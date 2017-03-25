using UnityEngine;
using System.Collections;

[System.Serializable]
public class DiceResult
{
    public int DieOne;
    public int DieTwo; 
    
    public DiceResult(int one, int two)
    {
        DieOne = one;
        DieTwo = two;
    }  
}
