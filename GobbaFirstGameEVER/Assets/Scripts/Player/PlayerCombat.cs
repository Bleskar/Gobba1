using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, IKillable
{
    public static PlayerCombat Instance { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerInventory Inventory { get; private set; }

    Camera cam;

    PlayerAnimation anim;

    public bool controller;

    [Header("Player Stats")]
    [SerializeField] int initMaxHealth = 100;
    public int MaxHealth
    {
        get
        {
            int itemBoost = 0;

            foreach (StatItem i in Inventory.Items)
            {
                itemBoost += i.healthBoost;
            }

            return initMaxHealth + itemBoost;
        }
    }

    int health;
    public int Health
    {
        get => health;
        set
        {
            if (value > MaxHealth)
                value = MaxHealth;
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
    public Weapon Holding => Inventory?.Holding;

    public bool Attacking { get; private set; }
    public bool CanAttack => !Movement.Frozen && !Attacking && !Dead && !Damaged;

    public bool Dead { get; private set; }

    float damageTimer;
    float iTime;
    public bool Damaged => damageTimer > 0f;

    Rigidbody2D rb;

    //used when attacking
    public Vector3 MouseDirection => (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    Vector3 direction;

    private void Awake()
    {
        direction = new Vector2(1f,0f);
        Instance = this;
        Movement = GetComponent<PlayerMovement>();
        Inventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        cam = Camera.main;
        anim = GetComponent<PlayerAnimation>();

        health = MaxHealth;
        slashSprite = slashAnimation.GetComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (damageTimer > 0f)
            damageTimer -= Time.deltaTime;
        else if (iTime > 0f)
            iTime -= Time.deltaTime;

        sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime * 8f);

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
        AudioManager.Play("Slash");

        Attacking = true;

        if (!controller)
        {
            direction = MouseDirection.normalized;
        }
        else
        {
            Vector2 temp = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).normalized;
            if (temp != Vector2.zero)
            {
                direction = temp;
            }
        }
        anim.lastDirection = direction;
        direction.z = 0f;
        direction.Normalize();

        slashAnimation.gameObject.SetActive(true);

        //animation stuff
        slashAnimation.speed = 1f / Holding.attackTime;
        slashAnimation.Play("Slash");

        //position rotation and scale
        slashAnimation.transform.position = transform.position + direction * (Holding.attackRadius + Holding.attackOffset);
        slashAnimation.transform.localScale = Vector3.one * Holding.attackRadius * 2f;
        slashAnimation.transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.Atan(direction.y / direction.x) * 180f) / Mathf.PI);
        slashSprite.flipX = direction.x < 0f;

        yield return new WaitForSeconds(Holding.attackTime / 2f);

        AttackHitBox(direction);


        yield return new WaitForSeconds(Holding.attackTime / 2f);
        slashAnimation.gameObject.SetActive(false);

        yield return new WaitForSeconds(Holding.attackCooldown);
        Attacking = false;
    }

    void AttackHitBox(Vector3 direction)
    {
        Collider2D[] cda = Physics2D.OverlapCircleAll(transform.position + direction * (Holding.attackRadius + Holding.attackOffset), Holding.attackRadius, enemyLayer);
        for (int i = 0; i < cda.Length; i++)
        {
            IKillable ik = cda[i].GetComponent<IKillable>();
            if (ik != null) 
            {
                cam.GetComponent<CameraShaker>().Shake();
                ik.Damage(Holding.damage, direction);  
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Holding)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.right * (Holding.attackRadius + Holding.attackOffset), Holding.attackRadius);
    }

    public void Damage(int dmg, Vector2 knock)
    {
        if (iTime > 0f || Health <= 0)
            return;

        Health -= dmg;
        sr.color = Color.red;
        rb.velocity = knock.normalized * 5f;

        AudioManager.Play("PlayerHurt");

        anim.Play("Damage");
        damageTimer = .2f;
        iTime = .2f;

        if (Health <= 0)
            Kill();
    }

    public void Kill()
    {
        if (Dead)
            return;

        anim.Play("Death");
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Dead = true;
        DeathMenu.Instance.Activate();
    }
}
