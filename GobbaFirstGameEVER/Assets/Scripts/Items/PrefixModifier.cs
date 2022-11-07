using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefixModifier
{
    public string prefix;

    [Header("Stat boosts")]
    public int damage;
    public int health;
    public int speed;
    public int attackSpeed;
}
