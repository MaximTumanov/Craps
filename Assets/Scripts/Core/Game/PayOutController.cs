using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PayOutController : MonoBehaviour
{

    //public int Pass = 20;
    //public int DontPass = 20;
    //public int[] PointPromise = new int[6] {4,5,6,8,9,10}; 
    //public int[] Field = new int[5] {3,4,9,10,11};
    //public int[]

    public PassCell Pass = new PassCell();
    public DontPassCell DontPass = new DontPassCell();
    public ComeCell Come = new ComeCell();
    public DontComeCell DontCome = new DontComeCell();
    public FieldCell Field = new FieldCell();    
    public PointCell[] Point;

    public SevenHardCell Seven = new SevenHardCell();
    public AnyCrapsCell AnyCraps = new AnyCrapsCell();

    public HardwayCell[] Hardway;
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
    
    public void UpdateState()
    {

    }

    public void PayoutPlayer(Player player)
    {
        for (int i = 0; i < player.Bets.Count; i++)
        {

        }
    }
    
    public void FindCheckCell()
    {

    }
}
