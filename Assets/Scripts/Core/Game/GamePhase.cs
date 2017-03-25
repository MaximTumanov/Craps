using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GamePhase
{
    public List<GameAction> GameActions;
    
    public void DoAction(int actionId, DiceResult parametrs)
    {
        for (int i = 0; i < GameActions.Count; i++)
        {
            if(GameActions[i].Id == actionId)
            {
                GameActions[i].Action(parametrs);
                return;
            }
        }
    }

}
