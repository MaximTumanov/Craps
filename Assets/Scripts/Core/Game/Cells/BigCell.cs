using UnityEngine;
using System.Collections;

[System.Serializable]
public class BigCell : BaseCell
{
    public BigCell(int number) : base()
    {
        Name = Cell.Big + number;
    }
}
