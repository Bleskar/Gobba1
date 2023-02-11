using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBowScript : MonoBehaviour
{
    public GameObject projectile;
    public int arrows;
    public int maxArrows;
    public int damage;
    public float force;
    public float attackOffset;
    public LayerMask playerLayer;

    public float timeElapsed;
    public float timeDelay;
    private void Start()
    {
        arrows = maxArrows;
    }
    public void Attack(Vector3 direction)
    {
        direction.z = 0f;
        direction.Normalize();
        Shoot(direction);
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime;
    }
    void Shoot(Vector3 direction)
    {
        if (timeElapsed > timeDelay && arrows == 0)
        {
            arrows = maxArrows;
        }
        else if (arrows != 0)
        {
            arrows--;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            GameObject arrow = Instantiate(projectile, (transform.position + direction * attackOffset), q);

            Projectile Pr = arrow.GetComponent<Projectile>();
            Pr.direction = direction;
            if (arrows == 0)
            {
                timeElapsed = 0f;
                return;
            }
        }
    }
}
