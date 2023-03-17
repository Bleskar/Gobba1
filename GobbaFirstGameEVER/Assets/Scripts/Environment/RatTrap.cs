using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatTrap : MonoBehaviour
{
    [SerializeField] float aliveTime = 5f;
    SpriteRenderer sr;

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase eb = collision.GetComponent<EnemyBase>();
        if (eb)
        {
            eb.Damage(PlayerCombat.Instance.AttackDamage, collision.transform.position - transform.position, null);
            Destroy(gameObject);
        }
    }
}
