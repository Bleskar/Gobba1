using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EnemyBase
{
    [Header("Options")]
    [SerializeField] float acceleration = 50f;
    [SerializeField] float deacceleration = 50f;
    [SerializeField] float topSpeed = 8f;

    private void Update()
    {
        sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime * 8f);
    }

    public void Movement(float x, float y) => Movement(new Vector2(x, y));
    public void Movement(Vector2 input)
    {
        input.Normalize();

        if (input != Vector2.zero)
        {
            //moving
            rb.velocity += input * Time.deltaTime * acceleration;
            if (rb.velocity.magnitude > topSpeed)
                rb.velocity = rb.velocity.normalized * topSpeed;
            return;
        }
        //breaking
        Vector2 direction = rb.velocity.normalized;
        rb.velocity -= direction * Time.deltaTime * deacceleration;

        if (rb.velocity.normalized != direction)
            rb.velocity = Vector2.zero;
    }
    public override void Damage(int dmg, Vector2 knockback)
    {
        sr.color = Color.red;
        base.Damage(dmg, knockback);
    }
}
