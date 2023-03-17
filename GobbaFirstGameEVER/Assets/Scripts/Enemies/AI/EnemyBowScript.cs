using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBowScript : MonoBehaviour
{
    //Animation references
    [SerializeField] CharacterAnimator caAnim;
    [SerializeField] Animator anim;
    [SerializeField] EnemyAI ai;
    [SerializeField] SpriteRenderer sr;
    float shootAnimation;

    public GameObject projectile;
    public float arrows;
    public float maxArrows;
    public int damage;
    public float force;
    public float attackOffset;
    public LayerMask playerLayer;

    public float timeElapsed;
    public float timeDelay;
    private void Start()
    {
        arrows = 0;
        maxArrows = 3 * Mathf.Exp((GameManager.Instance.CurrentLevel - 1) / 4);
        Mathf.RoundToInt(maxArrows);
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

        if (shootAnimation <= 0f)
        {
            if (ai.movementInput == Vector2.zero)
                caAnim.Play("Idle");
            else
                caAnim.Play("Walk");
        }
        else
            shootAnimation -= Time.deltaTime;

        if (ai.movementInput.x != 0f)
            sr.flipX = ai.movementInput.x < 0f;
    }
    void Shoot(Vector3 direction)
    {
        timeDelay /= Mathf.Exp((GameManager.Instance.CurrentLevel - 1) / 4);
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

            ShootAnimation();

            Projectile Pr = arrow.GetComponent<Projectile>();
            Pr.direction = direction;
            if (arrows == 0)
            {
                timeElapsed = 0f;
                return;
            }
        }
    }

    void ShootAnimation()
    {
        shootAnimation = .25f;
        anim.Play("Shoot");
    }
}
