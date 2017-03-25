using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Player[] Players;
    public Shooter Shooter;
    public PayOutController PayoutController;
    public GamePhase CurrentGamePhase;

    public int CurrentPointState = -1; 

    public GamePhase ComeOutRollPhase;
    public GamePhase PointPhase;

    void Start()
    {
        ComeOutRollPhase = new GamePhase();
        ComeOutRollPhase.GameActions = new List<GameAction>()
        {
            {new GameAction(Numbers.Seven,ShooterWin)},
            {new GameAction(Numbers.Elevent,ShooterWin)}
        };
        for (int i = 0; i < Numbers.Craps.Length; i++)
        {
            ComeOutRollPhase.GameActions.Add(new GameAction(Numbers.Craps[i],ShooterLose));
        }
        for (int i = 0; i < Numbers.Point.Length; i++)
        {
            int number = Numbers.Point[i];
            ComeOutRollPhase.GameActions.Add(new GameAction(number,SetPoint));
        }

        PointPhase = new GamePhase();
        PointPhase.GameActions = new List<GameAction>()
        {
            {new GameAction(Numbers.Seven,ShooterLose)}
        };
        for (int i = 0; i < Numbers.Point.Length; i++)
        {
            int number = Numbers.Point[i];
            ComeOutRollPhase.GameActions.Add(new GameAction(number,SetPoint));
        }
    }

    public void ShooterWin(DiceResult result)
    {
        CurrentPointState = -1;
        CurrentGamePhase = ComeOutRollPhase;
    }

    public void ShooterLose(DiceResult result)
    {
        CurrentPointState = -1;
        CurrentGamePhase = ComeOutRollPhase;
    }

    public void SetPoint(DiceResult result)
    {
        CurrentPointState = result.DieOne + result.DieTwo;
        CurrentGamePhase = PointPhase;
    }

    public void NextStep(DiceResult result)
    {

    }

    public void CulculatePayOut(DiceResult result)
    {

    }

    public void CheckPoint(DiceResult result)
    {

    }


}
