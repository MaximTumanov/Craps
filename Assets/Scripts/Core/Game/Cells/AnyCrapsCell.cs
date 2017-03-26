using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnyCrapsCell : BaseCell
{

    public AnyCrapsCell() : base()
    {
        Coefficient = 7;
        Name = Cell.AnyCraps;
    }

    public override CellResult Check(DiceResult result, ShooterState shooter, string phase)
    {
        int value = result.DieOne + result.DieTwo;
        for (int i = 0; i < Numbers.Craps.Length; i++)
        {
            if (value == Numbers.Craps[i])
                return CellResult.Won;   
        }
        return CellResult.Lost;
    }
}
