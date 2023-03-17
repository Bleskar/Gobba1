using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPool : MonoBehaviour
{
    [SerializeField] int damage = 5;
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
        PlayerCombat pc = collision.GetComponent<PlayerCombat>();
        pc?.Damage(damage, collision.transform.position - transform.position, null);
    }
}
