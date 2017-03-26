using UnityEngine;
using System.Collections;

[System.Serializable]
public class HardwayCell : BaseCell
{
    public DiceResult DiceResult;

    public HardwayCell(DiceResult diceResult):base()
    {
        Name = Cell.Hardway + diceResult.DieOne + diceResult.DieTwo;
        DiceResult = diceResult;
        if( (diceResult.DieOne ==2 && diceResult.DieTwo == 2)
            || (diceResult.DieOne == 5 && diceResult.DieTwo == 5))
        {
            Coefficient = 7;
        }else if(
                (diceResult.DieOne == 3 && diceResult.DieTwo == 3)
            ||  (diceResult.DieOne == 4 && diceResult.DieTwo == 4))
        {
            Coefficient = 9;
        }
    }

    public override CellResult Check(DiceResult result, ShooterState shooter, string phase)
    {
        if(result.DieOne == DiceResult.DieOne && result.DieTwo == DiceResult.DieTwo)
        {
            return CellResult.Won;
        }
        if(result.DieOne + result.DieTwo == DiceResult.DieOne + DiceResult.DieTwo)
        {
            return CellResult.Lost;
        }
        return CellResult.Next;
    }
}
