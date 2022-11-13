using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordAttack : MonoBehaviour
{
    public int damage;
    public float attackRadius;
    public float attackOffset;
    public LayerMask playerLayer;

    public void Attack(Vector3 direction)
    {
        direction.z = 0f;
        direction.Normalize();
        AttackHitBox(direction);
    }
    void AttackHitBox(Vector3 direction)
    {
        Collider2D[] cda = Physics2D.OverlapCircleAll(transform.position + direction * (attackRadius + attackOffset), attackRadius, playerLayer);
        for (int i = 0; i < cda.Length; i++)
        {
            IKillable ik = cda[i].GetComponent<IKillable>();
            if (ik != null) ik.Damage(damage, direction);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.right * (attackRadius + attackOffset), attackRadius);
    }
}
