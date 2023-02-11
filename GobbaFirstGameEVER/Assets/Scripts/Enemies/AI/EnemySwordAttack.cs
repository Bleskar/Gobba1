using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordAttack : MonoBehaviour
{
    public int damage;
    public float attackRadius;
    public float attackOffset;
    public LayerMask playerLayer;
    Camera cam;


    [Header("Attack Information")]
    [SerializeField] Animator slashAnimation;
    SpriteRenderer slashSprite;
    public float attackTime;
    public float attackDelay;

    private void Start()
    {
        cam = Camera.main;
        slashSprite = slashAnimation.GetComponent<SpriteRenderer>();
    }

    public void Attack(Vector3 direction)
    {
        direction.z = 0f;
        direction.Normalize();
        StartCoroutine(AttackAnimation(direction));
    }
    IEnumerator AttackAnimation(Vector3 direction)
    {
        yield return new WaitForSeconds(attackDelay);

        slashAnimation.gameObject.SetActive(true);

        //animation stuff
        slashAnimation.speed = 1f / attackTime;
        slashAnimation.Play("Slash");

        //position rotation and scale
        slashAnimation.transform.position = transform.position + direction * (attackRadius + attackOffset);
        slashAnimation.transform.localScale = Vector3.one * attackRadius * 2f;
        slashAnimation.transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.Atan(direction.y / direction.x) * 180f) / Mathf.PI);
        slashSprite.flipX = direction.x < 0f;

        yield return new WaitForSeconds(attackTime / 2f);

        AttackHitBox(direction);

        yield return new WaitForSeconds(attackTime / 2f);
        slashAnimation.gameObject.SetActive(false);
    }
    void AttackHitBox(Vector3 direction)
    {
        Collider2D[] cda = Physics2D.OverlapCircleAll(transform.position + direction * (attackRadius + attackOffset), attackRadius, playerLayer);
        for (int i = 0; i < cda.Length; i++)
        {
            IKillable ik = cda[i].GetComponent<IKillable>();
            if (ik != null)
            {
                if (cda[i].gameObject != gameObject)
                {
                    cam.GetComponent<CameraShaker>().Shake();
                    ik.Damage(damage, direction);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.right * (attackRadius + attackOffset), attackRadius);
    }
}
