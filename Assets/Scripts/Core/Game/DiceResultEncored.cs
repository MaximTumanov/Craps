using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceResultCodes : ScriptableObject
{
    public DiceResultCode[] Codes;

    public int GetCode(int[] result)
    {
        for (int i = 0; i < Codes.Length; i++)
        {
           if(CompareResults(Codes[i].results,result))
              return Codes[i].Id;
        }
        return -1;
    }

    public bool CompareResults(int[] code,int[] dice)
    {
        List<int> exclude = new List<int>();
        return true;
    }

    [System.Serializable]
    public class DiceResultCode
    {
        public int Id;
        public int[] results;
    }
}
