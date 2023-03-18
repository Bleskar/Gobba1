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
    SmokeDash, //inte tar skada n�r man dashar
    TrapTrail, //l�gger ut f�llor efter sig
    Multishot, //skjuter tre projectiles
    Cactus, //G�r skada tillbaka n�r man tar skada
    RocketTrap, //f�llor, fast raket :)
}
