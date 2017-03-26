using UnityEngine;
using System.Collections;

public class SevenHardCell : BaseCell
{

	public SevenHardCell ():base()
	{
		Name = Cell.Seven;
		Coefficient = 4;
	}

	public override CellResult Check (DiceResult result, ShooterState shooter)
	{	
		if ((result.DieOne + result.DieTwo) == 7)
			return CellResult.Won;

		return CellResult.Lost;
	}
}
