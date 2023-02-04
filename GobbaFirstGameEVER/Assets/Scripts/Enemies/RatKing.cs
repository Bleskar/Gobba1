using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatKing : EnemyBase
{
    bool alive;
    [SerializeField] Projectile ratling;
    [SerializeField] float speed = 5f;

    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        StartCoroutine(Phases());
    }

    private void Update()
    {
        sr.flipX = PlayerMovement.Instance.transform.position.x < transform.position.x;
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

            PlayAnimation("Idle");
            yield return new WaitForSeconds(1f);

            PlayAnimation("Summon");

            for (int i = 0; i < Random.Range(3, 4); i++)
            {
                yield return new WaitForSeconds(.5f);

                AudioManager.Play("Summon");
                Shoot();
            }

            PlayAnimation("Idle");
            yield return new WaitForSeconds(1f);
        }
    }

    public override void Kill()
    {
        alive = false;
        StopAllCoroutines();
        base.Kill();
    }

    public void Shoot()
    {
        Projectile clone = Instantiate(ratling, transform.position, Quaternion.identity);
        clone.transform.position += (Vector3)(Random.insideUnitCircle.normalized) * 3f;
        clone.transform.position += Vector3.back * 3f;
    }
}
