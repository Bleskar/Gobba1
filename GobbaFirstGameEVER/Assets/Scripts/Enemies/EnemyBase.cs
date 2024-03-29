using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IKillable
{
    [Header("Stats")]
    public float maxHealth = 5;

    public float Health { get; private set; }

    //HELLO EVERYBODY MY NAME IS MULTIPLIER
    protected float Multiplier => Mathf.Exp((GameManager.Instance.CurrentLevel - 1) / 4);
    
    //--ANIMATION--
    public string CurrentAnimation { get; private set; }

    //--REFRENCES--
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected CharacterAnimator anim;

    //used when player enters room to reset the enemy's position
    [HideInInspector] public Vector3 startPosition;

    private void Start()
        => Initialize();

    protected void Initialize()
    {
        maxHealth *= Multiplier;
        Health = maxHealth;

        anim = GetComponent<CharacterAnimator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public virtual void Damage(int dmg, Vector2 knockback, IKillable attacker)
    {
        AudioManager.Play("Hurt");

        if (Health > 0)
            Health -= dmg;
        if (Health <= 0)
        {
            Health = 0;
            Kill();
        }
            
    }

    public virtual void Kill()
    {
        GameManager.Instance.enemiesKilled++;
        GameManager.Instance.PlayParticles("DeathPoof", transform.position, Random.Range(6, 12));

        if (Random.value < .25f)
            GameManager.Instance.DropHeart(transform.position, PlayerMovement.Instance.CurrentRoom);

        Destroy(gameObject);
    }

    public void PlayAnimation(string animation)
    {
        if (animation == CurrentAnimation)
            return;

        CurrentAnimation = animation;
        anim.Play(animation);
    }

    public void RoomEnter()
    {
        transform.position = startPosition;
    }

    protected void AttackBox(Vector2 offset, Vector2 size, int damage)
    {
        Collider2D[] cda = Physics2D.OverlapBoxAll(transform.position + (Vector3)offset, size, 0f);
        for (int i = 0; i < cda.Length; i++)
        {
            PlayerCombat pc = cda[i].GetComponent<PlayerCombat>();
            if (pc) pc.Damage(damage, pc.transform.position - transform.position, this);
        }
    }
}
