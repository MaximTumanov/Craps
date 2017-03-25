using UnityEngine;
using System.Collections;

public class PointCell : BaseCell
{
    public PointCell(int number) : base()
    {
        Name = Cell.Point + number;
    }
}
