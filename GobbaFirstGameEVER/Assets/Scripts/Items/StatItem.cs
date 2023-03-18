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
    public int dashBoost;
    public int atkspd;
    public ItemSpecial special;
}

public enum ItemSpecial
{
    None,
    SmokeDash, //inte tar skada när man dashar
    TrapTrail, //lägger ut fällor efter sig
    Multishot, //skjuter tre projectiles
    Cactus, //Gör skada tillbaka när man tar skada
    RocketTrap, //fällor, fast raket :)
}
