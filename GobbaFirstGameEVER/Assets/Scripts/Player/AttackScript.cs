using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public float attackRange;
    public float offset;
    public Vector2 attackPoint;
    public LayerMask enemy;
    private void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            AttackBox();
        }
    }
    void AttackBox()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemy);
        foreach (var col in cols)
        {
            //Adda dmg/ knockback
        }
    }
}