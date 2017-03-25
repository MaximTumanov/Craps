using UnityEngine;
using System.Collections;

[System.Serializable]
public class PointCell : BaseCell
{
    public PointCell(int number) : base()
    {
        Name = Cell.Point + number;

		if (number == 4 || number == 10)
			Coefficient = 1.8f;
 		
		if (number == 5 || number == 9)
			Coefficient = 1.4f;

		if (number == 6 || number == 8)
			Coefficient = (float)(7/6);
		
	}
}
