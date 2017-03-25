using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhase : MonoBehaviour
{
    public List<GameAction> GameActions;

    public void DoAction(int actionId, object parametrs)
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
