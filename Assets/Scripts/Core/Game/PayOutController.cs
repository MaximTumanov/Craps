using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class PayOutController
{

    //[HideInInspector]
    public PassCell Pass = new PassCell();
    //[HideInInspector]
    public DontPassCell DontPass = new DontPassCell();
    //[HideInInspector]
    public ComeCell Come = new ComeCell();
    //[HideInInspector]
    public DontComeCell DontCome = new DontComeCell();
    [HideInInspector]
    public FieldCell Field = new FieldCell();
    [HideInInspector]
    public PointCell[] Point;
    [HideInInspector]
    public SevenHardCell Seven = new SevenHardCell();
    [HideInInspector]
    public AnyCrapsCell AnyCraps = new AnyCrapsCell();
    [HideInInspector]
    public HardwayCell[] Hardway;
    [HideInInspector]
    public HardwayExactCell[] HardwayExact;

    [HideInInspector]
    public BigCell[] Big;

    public string CurrentState;
    public int TablePointer = -1;

    public List<BaseCell> PayoutCells = new List<BaseCell>();  
    
    public Action<int,Bet,int> PayOutCallback; 
     
    public void Init()
    {
        PayoutCells.Clear();
        PayoutCells.Add(Pass);
        PayoutCells.Add(DontPass);
        PayoutCells.Add(Come);
        PayoutCells.Add(DontCome);
        PayoutCells.Add(Field);
        PayoutCells.Add(Seven);
        PayoutCells.Add(AnyCraps);
        Point = new PointCell[Numbers.Point.Length];
        for (int i = 0; i < Numbers.Point.Length; i++)
        {
            Point[i] = new PointCell(Numbers.Point[i]);
            PayoutCells.Add(Point[i]);
        }
        Hardway = new HardwayCell[Numbers.Hardway.Length];
        for (int i = 0; i < Numbers.Hardway.Length; i++)
        {
            Hardway[i] = new HardwayCell(Numbers.Hardway[i]);
            PayoutCells.Add(Hardway[i]);
        }
        HardwayExact = new HardwayExactCell[Numbers.HardwayExact.Length];
        for (int i = 0; i < Numbers.HardwayExact.Length; i++)
        {
            HardwayExact[i] = new HardwayExactCell(Numbers.HardwayExact[i]);
            PayoutCells.Add(HardwayExact[i]);
        }
        Big = new BigCell[Numbers.Big.Length];
        for (int i = 0; i < Numbers.Big.Length; i++)
        {
            Big[i] = new BigCell(Numbers.Big[i]);
            PayoutCells.Add(Big[i]);
        }

    }  
    
    public void UpdateState(Table table)
    {
        CurrentState = table.CurrentGamePhase.Name;
        TablePointer = table.TablePointState;
    }

    public void ShooterWon(Player player, DiceResult result)
    {
        List<Bet> betForRemove = new List<Bet>();
        for (int i = 0; i < player.Bets.Count; i++)
        {
            if(player.Bets[i].Name == Cell.Pass)
            {
                Pass.Payout(player.Bets[i].Amount, result);
                continue;
            }
            if (player.Bets[i].Name == Cell.DontPass)
            {
                betForRemove.Add(player.Bets[i]);
                continue;
            }
        }
        for (int i = betForRemove.Count; i > 0 ; i++)
        {
            player.Remove(betForRemove[i]);
        }
        betForRemove.Clear();
    }

    public void ShooterLose(Player player, DiceResult result)
    {

    }

    public void PayoutPlayer(Player player, DiceResult result, ShooterState shooter)
    {
        for (int i = player.Bets.Count - 1; i >= 0; i--)
        {
            player.Balance += CheckBetPayout(player, player.Bets[i], result, shooter);
        }
    }
    
    public int CheckBetPayout(Player player, Bet bet, DiceResult result, ShooterState shooter)
    {
        for (int i = 0; i < PayoutCells.Count; i++)
        {
            if(PayoutCells[i].Name == bet.Name)
            {
                CellResult checkResult = PayoutCells[i].Check(result, shooter, CurrentState);
                if(checkResult == CellResult.Won)
                {
                    int amount = (int)PayoutCells[i].Payout(bet.Amount,result);
                    PayOutCallback(player.Id,bet,amount);
				    return amount;//(int)PayoutCells[i].Payout(bet.Amount,result);
                }
                if (checkResult == CellResult.Lost)
                {
                    player.Remove(bet);
                    return 0;
                }
            }
        }
        return 0;
    }
}
