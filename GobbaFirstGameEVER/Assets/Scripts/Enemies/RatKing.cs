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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Phases()
    {
        alive = true;
        while (alive)
        {
            for (int i = 0; i < Random.Range(3, 6); i++)
            {
                yield return new WaitForSeconds(.5f);

                Vector2 dir = ((Vector2)PlayerMovement.Instance.transform.position - (Vector2)transform.position).normalized;
                Shoot(dir);
                Shoot(dir + Vector2.Perpendicular(dir));
                Shoot(dir - Vector2.Perpendicular(dir));
            }

            yield return new WaitForSeconds(1f);

            direction = Random.insideUnitCircle.normalized;
        }
    }

    public override void Kill()
    {
        alive = false;
        StopAllCoroutines();
        base.Kill();
    }

    public void Shoot(Vector2 dir)
    {
        Projectile clone = Instantiate(ratling, transform.position, Quaternion.identity);
        clone.direction = dir.normalized;
    }
}
