using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PayOutController
{

    //public int Pass = 20;
    //public int DontPass = 20;
    //public int[] PointPromise = new int[6] {4,5,6,8,9,10}; 
    //public int[] Field = new int[5] {3,4,9,10,11};
    //public int[]
    [HideInInspector]
    public PassCell Pass = new PassCell();
    [HideInInspector]
    public DontPassCell DontPass = new DontPassCell();
    [HideInInspector]
    public ComeCell Come = new ComeCell();
    [HideInInspector]
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
    
    public List<BaseCell> PayoutCells = new List<BaseCell>();  
     
    public void Init()
    {
        PayoutCells.Clear();
        PayoutCells.Add(Pass);
        PayoutCells.Add(DontPass);
        PayoutCells.Add(Come);
        PayoutCells.Add(DontCome);
        PayoutCells.Add(Field);
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
    }  
    
    public void UpdateState(Table table)
    {

    }

    public void PayoutPlayer(Player player, DiceResult result)
    {
        for (int i = 0; i < player.Bets.Count; i++)
        {
            player.Balance += CheckBetPayout(player, player.Bets[i], result);
        }
    }
    
    public int CheckBetPayout(Player player,Bet bet, DiceResult result)
    {
        for (int i = 0; i < PayoutCells.Count; i++)
        {
            if(PayoutCells[i].Name == bet.Name)
            {
				return (int)PayoutCells[i].Payout(bet.Amount,result);
            }
        }
        return 0;
    }
}
