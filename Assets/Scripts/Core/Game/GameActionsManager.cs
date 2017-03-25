using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[Serializable]
public class GameAction
{
    public int Id;
    public Action<object> Action;

    public GameAction(int id, Action<object> action)
    {
        Id = id;
        Action = action;
    }
}

