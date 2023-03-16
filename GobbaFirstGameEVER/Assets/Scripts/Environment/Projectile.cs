using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float radius = .25f;

    public Vector3 direction;
    public float speed = 20f;
    public int damage;

    public bool faceDirection;
    public bool Travelling => travelling && !dead;

    float deathTimer;
    bool dead;
    public bool travelling;

    SpriteRenderer sr;

    private void Start()
    {
        deathTimer = 20f;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.enabled = !dead;

        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
        {
            Kill();
        }

        if (Travelling)
            Travel();
    }

    public void Travel()
    {
        if (faceDirection)
            transform.right = direction;

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, direction.normalized, speed * Time.deltaTime, targetLayers);
        if (hit)
        {
            IKillable pc = hit.transform.GetComponent<IKillable>();
            if (pc != null)
                pc.Damage(damage, direction);

            transform.position = hit.point;
            Kill();
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void Kill()
    {
        if (!dead)
        {
            deathTimer = 2f;
            dead = true;
            return;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
