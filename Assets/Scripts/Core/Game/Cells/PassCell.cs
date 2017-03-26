using UnityEngine;
using System.Collections;

[System.Serializable]
public class PassCell : BaseCell
{
    public PassCell():base()
    {
        Name = Cell.Pass;
    }

    public override CellResult Check(DiceResult result, ShooterState shooter, string phases)
    {
        return base.Check(result, shooter, phases);
    }
}
