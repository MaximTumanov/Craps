using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Player[] Players;
    public Shooter Shooter;
    public PayOutController PayoutController;
    public GamePhase CurrentGamePhase;

    public int TablePointState = -1; 

    public GamePhase ComeOutRollPhase;
    public GamePhase PointRollPhase;

    void Start()
    {
        PayoutController.Init();

        ComeOutRollPhase = new GamePhase();
        ComeOutRollPhase.Name = Phases.ComeOut;
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

        PointRollPhase = new GamePhase();
        PointRollPhase.Name = Phases.Point;
        PointRollPhase.GameActions = new List<GameAction>()
        {
            {new GameAction(Numbers.Seven,ShooterLose)}
        };
        for (int i = 2; i < 13; i++)
        {
            if(i == Numbers.Seven || i == Numbers.Elevent)
                continue;
            PointRollPhase.GameActions.Add(new GameAction(i,NextStep));
        }
        CurrentGamePhase = ComeOutRollPhase;
    }

    [ContextMenu ("DoTestThrow")]
    public void DoTestThrow()
    {
        DiceResult result = new DiceResult(Random.Range(1,7),Random.Range(1,7));
        ApplyResult(result);
    }

    public void ApplyResult(DiceResult result)
    {
        CurrentGamePhase.DoAction(result.DieOne+result.DieTwo, result);
    }

    public void ShooterWin(DiceResult result)
    {
        TablePointState = -1;
        CurrentGamePhase = ComeOutRollPhase;
        UpdatePayout();
    }

    public void ShooterLose(DiceResult result)
    {
        TablePointState = -1;
        CurrentGamePhase = ComeOutRollPhase;
        UpdatePayout();

    }

    public void SetPoint(DiceResult result)
    {
        TablePointState = result.DieOne + result.DieTwo;
        CurrentGamePhase = PointRollPhase;
        UpdatePayout();
    }

    public void NextStep(DiceResult result)
    {
        UpdatePayout();
    }

    public void UpdatePayout()
    {
        PayoutController.UpdateState(this);
    }
}
