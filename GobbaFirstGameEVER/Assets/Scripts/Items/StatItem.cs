using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Stat Booster")]
public class StatItem : ItemBase
{
    [Header("Stat Boosts")]
    public int healthBoost;
    public int atkBoost;
    public int speedBoost;
    public int atkspd;
}
