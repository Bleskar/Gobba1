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

    protected float deathTimer;
    protected bool dead;
    public bool travelling;
    public bool alwaysShow;

    protected SpriteRenderer sr;

    private void Start()
    {
        deathTimer = 20f;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.enabled = !dead || alwaysShow;

        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
        {
            Kill(null);
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
            IKillable ik = hit.transform.GetComponent<IKillable>();
            if (ik != null)
                ik.Damage(damage, direction, null);

            transform.position = hit.point;
            Kill(ik);
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void Kill(IKillable ik)
    {
        if (!dead && ik == null)
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
