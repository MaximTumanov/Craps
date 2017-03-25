using UnityEngine;
using System.Collections;

[System.Serializable]
public class FieldCell : BaseCell
{
    public FieldCell() : base()
    {
        Name = Cell.Field;
    }
}
