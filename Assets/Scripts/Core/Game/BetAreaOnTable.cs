using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BetAreaOnTable : MonoBehaviour
{
    public static string[] PossibleBetNames = new string[] { "Pass", "DontPass", "Come", "DontCome", "Field", "Seven", "AnyCraps",
        "Point4", "Point5", "Point6", "Point8", "Point9", "Point10",
        "Hardway22", "Hardway33", "Hardway44", "Hardway55", "Hardway11", "Hardway66", "Hardway12", "Hardway56",
        "Big6", "Big8" };
    [HideInInspector] public string BetName;
   
}
