using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : EnemyBase
{
    [SerializeField] float speed = 5f;

    [SerializeField] float walkTimeMin = 3f;
    [SerializeField] float walkTimeMax = 5f;

    [SerializeField] LayerMask walls;

    [SerializeField] GameObject poisonPool;
    [SerializeField] float poolInterval = 3f;
    float timer;

    float walkTimer;
    Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer += poolInterval;
            Instantiate(poisonPool, transform.position + Vector3.forward, Quaternion.identity);
        }

        if (direction.y > 0f)
            anim.Play("CrawlUp");
        else if (direction.y < 0f)
            anim.Play("CrawlDown");
        else
        {
            sr.flipX = direction.x < 0f;
            anim.Play("CrawlSide");
        }

        if (walkTimer > 0f)
            walkTimer -= Time.deltaTime;
        else
            ChangeDirection();

        CheckWalls();
        rb.velocity = direction * speed;
    }

    void CheckWalls()
    {
        Vector3 dir = new Vector3(direction.x, direction.y);

        if (Physics2D.OverlapBox(transform.position + dir * .1f, Vector2.one, 0f, walls))
            ChangeDirection();
    }

    void ChangeDirection()
    {
        walkTimer = Random.Range(walkTimeMin, walkTimeMax);

        int ran = Random.Range(0, 4);
        if (ran == 0)
            direction = Vector2.right;
        else if (ran == 1)
            direction = Vector2.down;
        else if (ran == 2)
            direction = Vector2.left;
        else if (ran == 3)
            direction = Vector2.up;
    }
}
