using UnityEngine;
using System.Collections;

public class HardwayCell : BaseCell
{
    public DiceResult DiceResult;

    public HardwayCell(DiceResult diceResult):base()
    {
        Name = Cell.Hardway + diceResult.DieOne + diceResult.DieTwo;
        DiceResult = diceResult;
    }
}
