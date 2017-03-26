using System;
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

    public Action<int,Bet,int> PayOutCallback = EmptyPayOutCallback;  

    [ContextMenu ("Init")]
    void Init()
    {
        PayoutController.Init();
        PayoutController.PayOutCallback = PayOutCallback;
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
        DiceResult result = new DiceResult(UnityEngine.Random.Range(1,7), UnityEngine.Random.Range(1,7));
        ApplyResult(result);
    }

    public void ApplyResult(DiceResult result)
    {
        Debug.Log("Phase:"+CurrentGamePhase.Name+" ApplyThrow " + result.DieOne + " " + result.DieTwo );
        CurrentGamePhase.DoAction(result.DieOne+result.DieTwo, result);
    }

    public void ShooterWin(DiceResult result)
    {
        Debug.Log("ShooterWin " + (result.DieOne + result.DieTwo));
        
        TablePointState = -1;
        CurrentGamePhase = ComeOutRollPhase;
        
        PayoutPlayers(result, ShooterState.won);
        UpdatePayout();
    }

    public void ShooterLose(DiceResult result)
    {
        Debug.Log("ShooterLose " + (result.DieOne + result.DieTwo));
        
        TablePointState = -1;
        CurrentGamePhase = ComeOutRollPhase;
        PayoutPlayers(result, ShooterState.lost);
        UpdatePayout();

    }

    public void SetPoint(DiceResult result)
    {
        Debug.Log("SetPoint " + (result.DieOne + result.DieTwo));

        TablePointState = result.DieOne + result.DieTwo;
        CurrentGamePhase = PointRollPhase;
        PayoutPlayers(result, ShooterState.nothing);
        UpdatePayout();
    }

    public void NextStep(DiceResult result)
    {
        Debug.Log("NextStep " + (result.DieOne + result.DieTwo));

        PayoutPlayers(result, ShooterState.nothing);
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

    public void PayoutPlayers(DiceResult result, ShooterState shooter)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            PayoutController.PayoutPlayer(Players[i], result, shooter);
        }
    }

    public void EmptyPayOutCallback(int id, Bet bet,int amount) { }
}
