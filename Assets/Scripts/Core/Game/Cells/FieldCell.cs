using UnityEngine;
using System.Collections;

[System.Serializable]
public class FieldCell : BaseCell
{
	

    public FieldCell() : base()
    {
        Name = Cell.Field;

    }

	public override CellResult Check (DiceResult result, ShooterState shooter)
	{
		
		for (int i = 0; i < Numbers.FieldsX1.Length; i++)
		{
			if ((result.DieOne + result.DieTwo) == Numbers.FieldsX1 [i])
			{
				return CellResult.Won;
			}
		}

		return CellResult.Lost;

	}

	public override float Payout (int bet, DiceResult result)
	{		
		for (int i = 0; i < Numbers.FieldsSellMult_2.Length; i++) {
			if ((result.DieOne + result.DieTwo) == Numbers.FieldsSellMult_2 [i])
			{
				return 2 * base.Payout (bet, result);
			}
		}

		return base.Payout (bet, result);;
	}

}
