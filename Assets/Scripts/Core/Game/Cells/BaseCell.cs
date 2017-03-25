using UnityEngine;
using System.Collections;

[System.Serializable]
public class BaseCell
{
    public string Name;
    public int DieValue;
    public float Coefficient = 1;
    public BettingCellState BettingState;
    public PayCellState PayState;

    public virtual CellResult Check(DiceResult result)
    {
        CellResult value = CellResult.Next;
        if(DieValue == result.DieOne + result.DieTwo)
            value = CellResult.Won;
        return value;
    }

    public virtual float Payout (int bet)
    {
        return bet * Coefficient;
    }
}

public enum BettingCellState {Active, Blocked};
public enum PayCellState {Active, Blocked};
public enum CellResult {Won, Lost, Next};

public class Cell
{
    public const string Pass = "Pass";
    public const string DontPass = "DontPass";
    public const string Come = "Come";
    public const string DontCome = "DontPass";
    public const string Field = "Field";
    public const string Point = "Point";
    public const string Hardway = "Hardway";
    public const string HardwayExact = "HardwayExact";
    public const string Big = "Big";
    public const string Seven = "Seven";
    public const string AnyCraps = "AnyCraps";
}

public class Numbers
{
    public static int[] Point = new int[6] {4,5,6,8,9,10};
    public static int[] Craps = new int[3] {2,3,12};

    public static DiceResult[] Hardway = 
    {
        new DiceResult(2,2),
        new DiceResult(3,3),
        new DiceResult(4,4),
        new DiceResult(5,5)
    };

    public static DiceResult[] HardwayExact = 
    {
        new DiceResult(1,1),
        new DiceResult(6,6),
        new DiceResult(1,2),
        new DiceResult(5,6)
    };

    public const int Seven = 7;
    public const int Elevent = 11;
}

public class Phases
{
    public const string ComeOut = "ComeOut";
    public const string Point = "Point";
}