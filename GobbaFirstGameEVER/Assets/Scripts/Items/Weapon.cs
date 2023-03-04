using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Weapon")]
public class Weapon : ItemBase
{
    [Header("Base Stats")]
    public int damage;
    public float attackCooldown;
    public float attackTime;
    public float attackRadius = .75f;
    public float attackOffset = 0f;

    public override string Description => base.Description + $"\n+{damage} DMG";
}

