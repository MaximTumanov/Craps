using UnityEngine;
using System.Collections;

[System.Serializable]
public class ComeCell:BaseCell
{
    public ComeCell():base()
    {
        Name = Cell.Come;
    }

    public override CellResult Check(DiceResult result, ShooterState shooter)
    {
        int number = result.DieOne + result.DieTwo;
        if(number == Numbers.Elevent || number == Numbers.Seven)
        {
            return CellResult.Won;
        }
        for (int i = 0; i < Numbers.Craps.Length; i++)
        {
            if(number == Numbers.Craps[i])
            {
                return CellResult.Lost;
            }
        }
        return CellResult.Next;
    }
}
