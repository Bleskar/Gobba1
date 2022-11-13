using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBowScript : MonoBehaviour
{
    public GameObject projectile;

    public int damage;
    public float force;
    public float attackOffset;
    public LayerMask playerLayer;

    public void Attack(Vector3 direction)
    {
        direction.z = 0f;
        direction.Normalize();
        Shoot(direction);
    }
    void Shoot(Vector3 direction)
    {
        Debug.Log("skjuter mfer");
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject arrow = Instantiate(projectile, (transform.position + direction * attackOffset), q);

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * force);
    }
}
