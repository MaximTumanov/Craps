using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDie: MonoBehaviour
{
    public Action<int> OnCallback; 

    protected int Value;

    public virtual void ThrowDie(object parametrs, Action<int> callback)
    {
        Value = UnityEngine.Random.Range(0,7);
        if(OnCallback!=null)
            OnCallback(Value);
    }
}
