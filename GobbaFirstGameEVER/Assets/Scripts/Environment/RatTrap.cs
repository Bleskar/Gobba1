using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatTrap : MonoBehaviour
{
    [SerializeField] float aliveTime = 5f;
    [SerializeField] Sprite rocketSprite;
    [SerializeField] float acceleration;
    [SerializeField] Rigidbody2D rb;
    SpriteRenderer sr;

    EnemyBase target;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        aliveTime -= Time.deltaTime;

        if (aliveTime <= 1f)
        {
            sr.color = new Color(1f, 1f, 1f, aliveTime);

            if (aliveTime <= 0f)
                Destroy(gameObject);
        }
        else
            sr.color = Color.white;

        if (!PlayerInventory.Instance.HasPerk(ItemSpecial.RocketTrap))
            return;

        sr.sprite = rocketSprite;

        if (!target)
        {
            Search();
        }
        else
        {
            sr.flipX = rb.velocity.x < 0f;
            rb.velocity += ((Vector2)target.transform.position - (Vector2)transform.position).normalized * acceleration * Time.deltaTime;
        }
    }

    void Search()
    {
        Collider2D[] cda = Physics2D.OverlapCircleAll(transform.position, 11f);
        for (int i = 0; i < cda.Length; i++)
        {
            EnemyBase eb = cda[i].GetComponent<EnemyBase>();
            if (eb)
            {
                target = eb;
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase eb = collision.GetComponent<EnemyBase>();
        if (eb)
        {
            eb.Damage(PlayerCombat.Instance.AttackDamage * (3 + PlayerInventory.Instance.PerkLevel(ItemSpecial.TrapTrail) + PlayerInventory.Instance.PerkLevel(ItemSpecial.RocketTrap)), collision.transform.position - transform.position, null);
            Destroy(gameObject);
        }
    }
}
