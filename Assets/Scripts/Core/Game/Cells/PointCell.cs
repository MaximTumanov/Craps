using UnityEngine;
using System.Collections;

[System.Serializable]
public class PointCell : BaseCell
{
    public PointCell(int number) : base()
    {
        Name = Cell.Point + number;
    }
}
