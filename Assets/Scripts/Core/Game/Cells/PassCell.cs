using UnityEngine;
using System.Collections;

[System.Serializable]
public class PassCell : BaseCell
{
    public PassCell():base()
    {
        Name = Cell.Pass;
    }
}
