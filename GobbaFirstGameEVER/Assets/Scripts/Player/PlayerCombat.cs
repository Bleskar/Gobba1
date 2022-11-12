using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, IKillable
{
    public static PlayerCombat Instance { get; private set; }
    public PlayerMovement Movement { get; private set; }

    [Header("Player Stats")]
    [SerializeField] int maxHealth = 100;

    int health;
    public int Health
    {
        get => health;
        set
        {
            if (value > maxHealth)
                value = maxHealth;
            else if (value < 0)
                value = 0;

            health = value;
        }
    }

    [Header("Refrences")]
    [SerializeField] LayerMask enemyLayer;
    SpriteRenderer sr;

    [Header("Attack Information")]
    [SerializeField] Animator slashAnimation;
    SpriteRenderer slashSprite;
    public Weapon holding;

    public bool Attacking { get; private set; }
    public bool CanAttack => !Movement.Frozen && !Attacking;

    Rigidbody2D rb;

    //used when attacking
    public Vector3 MouseDirection => (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

    private void Awake()
    {
        Instance = this;
        Movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        slashSprite = slashAnimation.GetComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
            StartAttack();
    }

    public void StartAttack()
    {
        if (!CanAttack)
            return;

        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        Attacking = true;
        Vector3 direction = MouseDirection.normalized;
        direction.z = 0f;
        direction.Normalize();

        slashAnimation.gameObject.SetActive(true);

        //animation stuff
        slashAnimation.speed = 1f / holding.attackTime;
        slashAnimation.Play("Slash");

        //position rotation and scale
        slashAnimation.transform.position = transform.position + direction * (holding.attackRadius + holding.attackOffset);
        slashAnimation.transform.localScale = Vector3.one * holding.attackRadius * 2f;
        slashAnimation.transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.Atan(direction.y / direction.x) * 180f) / Mathf.PI);
        slashSprite.flipX = direction.x < 0f;

        yield return new WaitForSeconds(holding.attackTime / 2f);

        AttackHitBox(direction);

        yield return new WaitForSeconds(holding.attackTime / 2f);
        slashAnimation.gameObject.SetActive(false);

        yield return new WaitForSeconds(holding.attackCooldown);
        Attacking = false;
    }

    void AttackHitBox(Vector3 direction)
    {
        Collider2D[] cda = Physics2D.OverlapCircleAll(transform.position + direction * (holding.attackRadius + holding.attackOffset), holding.attackRadius, enemyLayer);
        for (int i = 0; i < cda.Length; i++)
        {
            IKillable ik = cda[i].GetComponent<IKillable>();
            if (ik != null) ik.Damage(holding.damage, direction);
        }
    }

    private void OnDrawGizmos()
    {
        if (!holding)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.right * (holding.attackRadius + holding.attackOffset), holding.attackRadius);
    }

    public void Damage(int dmg, Vector2 knock)
    {
        Health -= dmg;
        sr.color = Color.red;
        rb.velocity = knock.normalized * 5f;

        if (Health <= 0)
            Kill();
    }

    public void Kill()
    {
        print("Player dieded!!!! OH NO!!! :'(");
    }
}
