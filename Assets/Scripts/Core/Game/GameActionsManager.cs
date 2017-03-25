using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[Serializable]
public class GameAction
{
    public int Id;
    public Action<DiceResult> Action;

    public GameAction(int id, Action<DiceResult> action)
    {
        Id = id;
        Action = action;
    }
}

