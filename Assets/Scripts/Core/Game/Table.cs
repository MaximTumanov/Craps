using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<Player> Players;
    public Shooter Shooter;
    public PayOutController PayoutController;
    public GamePhase CurrentGamePhase;

    public int TablePointState = -1; 

    public GamePhase ComeOutRollPhase;
    public GamePhase PointRollPhase;

    [ContextMenu ("Init")]
    void Init()
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
        //PayoutController.ShooterWon();
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

    [ContextMenu ("AddPlayer")]
    public void AddPlayer()
    {
        AddPlayer(Random.Range(int.MinValue,int.MaxValue));
    }

    public void AddPlayer(int id)
    {
        Players.Add(new Player(id));
    }
    
    [ContextMenu ("RemovePlayer")]
    public void RemovePlayer()
    {
        if(Players.Count > 0)
        {
            RemovePlayer(Players[0].Id);
        }
    }

    public void RemovePlayer(int id)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if(Players[i].Id == id)
            {
                Players.RemoveAt(i);
                return;
            }
        }
    }

    [ContextMenu ("DoBet")]
    public void DoBet()
    {
        if(Players.Count > 0)
        {
            DoBet(Players[0].Id, new Bet(Cell.Field,100));
        }
    }

    public void DoBet(int playerId, Bet bet)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if(Players[i].Id == playerId)
            {
                Players[i].DoBet(bet);
            }
        }
    }

    public void PayoutPlayers(DiceResult result)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            PayoutController.PayoutPlayer(Players[i], result);
        }
    }
}
