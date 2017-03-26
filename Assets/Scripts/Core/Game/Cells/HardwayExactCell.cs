using UnityEngine;
using System.Collections;

public class HardwayExactCell : HardwayCell
{
    public HardwayExactCell(DiceResult result):base(result)
    {
        if( (result.DieOne == 1 && result.DieTwo == 1)
            || (result.DieOne == 6 && result.DieTwo == 6))
        {
            Coefficient = 30;
        }else if(
                (result.DieOne == 1 && result.DieTwo == 2)
            ||  (result.DieOne == 2 && result.DieTwo == 1)
            ||  (result.DieOne == 5 && result.DieTwo == 6)
            ||  (result.DieOne == 6 && result.DieTwo == 5))
        {
            Coefficient = 15;
        }
    }

    public override CellResult Check(DiceResult result, ShooterState shooter, string phase)
    {
        if((result.DieOne ==  DiceResult.DieOne && result.DieTwo == DiceResult.DieTwo)
            ||  (result.DieOne == DiceResult.DieTwo && result.DieTwo == DiceResult.DieOne))
        {
            return CellResult.Won;
        }
        return CellResult.Lost;
    }
}
