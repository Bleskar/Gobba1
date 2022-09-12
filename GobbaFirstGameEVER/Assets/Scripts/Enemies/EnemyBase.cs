using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IKillable
{
    [Header("Stats")]
    [SerializeField] int maxHealth = 5;

    public int Health { get; private set; }

    //--ANIMATION--
    public string CurrentAnimation { get; private set; }

    //--REFRENCES--
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    Animator anim;

    private void Start()
        => Initialize();

    protected void Initialize()
    {
        Health = maxHealth;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public virtual void Damage(int dmg, Vector2 knockback)
    {
        Health -= dmg;
        if (Health <= 0)
            Kill();
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public void PlayAnimation(string animation)
    {
        if (animation == CurrentAnimation)
            return;

        CurrentAnimation = animation;
        anim.Play(animation);
    }
}
