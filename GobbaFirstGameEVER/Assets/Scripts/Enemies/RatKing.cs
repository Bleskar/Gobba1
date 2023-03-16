using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatKing : EnemyBase
{
    bool alive;
    [SerializeField] Projectile ratling;
    [SerializeField] float speed = 5f;

    [SerializeField] GameObject ladder;
    
    Vector2 direction;

    bool initialized;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        StartCoroutine(Phases());
        initialized = true;
    }

    private void OnEnable()
    {
        if (initialized && alive)
            StartCoroutine(Phases());
    }

    private void Update()
    {
        sr.flipX = PlayerMovement.Instance.transform.position.x < transform.position.x;
        AttackBox(Vector2.zero, Vector2.one * 2f, 15 + 10 * Multiplier);
    }

    IEnumerator Phases()
    {
        alive = true;
        while (alive)
        {
            PlayAnimation("Walk");
            float timer = Random.Range(3f, 5f);
            while (timer > 0f)
            {
                timer -= Time.deltaTime;

                Vector2 dir = ((Vector2)PlayerMovement.Instance.transform.position - (Vector2)transform.position).normalized;
                rb.velocity = dir * speed;

                yield return null;
            }

            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            PlayAnimation("Idle");
            yield return new WaitForSeconds(1f);

            PlayAnimation("Summon");
            
            for (int i = 0; i < Random.Range(6 + 2 * Multiplier, 9 + 4 * Multiplier); i++)
            {
                yield return new WaitForSeconds(.5f/(.2f * (i + 6f)));

                AudioManager.Play("Summon");
                Shoot();
            }

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            PlayAnimation("Idle");
            yield return new WaitForSeconds(1f);
        }
    }

    public override void Kill()
    {
        if (!alive)
            return;

        alive = false;
        StopAllCoroutines();
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        PlayAnimation("Death");
        yield return new WaitForSeconds(1f);

        ladder.SetActive(true);
        Destroy(gameObject);
    }

    public void Shoot()
    {
        Projectile clone = Instantiate(ratling, transform.position, Quaternion.identity);
        clone.transform.position += (Vector3)(Random.insideUnitCircle.normalized) * 3f;
        clone.transform.position += Vector3.back * 3f;
    }
}
