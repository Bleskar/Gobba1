using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerCombat combat; 
    PlayerMovement mvmnt; 

    SpriteRenderer sr;
    
    Animator anim;
    string currentAnimation;

    [HideInInspector] public Vector2 lastDirection;

    public bool Animating => !PlayerCombat.Instance.Dead && !PlayerCombat.Instance.Damaged && !AnvilMenu.Instance.gameObject.activeSelf && !mvmnt.Dashing;

    // Start is called before the first frame update
    void Start()
    {
        combat = GetComponent<PlayerCombat>();
        mvmnt = GetComponent<PlayerMovement>();

        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Animating)
            return;

        if (!combat.Attacking)
        {
            Play(GetNormalAnimation());
        }
        else
        {
            Play(DirectionToString() + "Attack");
        }
    }

    string GetNormalAnimation()
    {
        string a = "Idle";

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        if (input != Vector2.zero)
        {
            lastDirection = input;
            a = "Walk";
        }

        a = DirectionToString() + a;

        return a;
    }

    public string DirectionToString()
    {
        string a = "Up";

        if (lastDirection.x != 0f && Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
        {
            sr.flipX = lastDirection.x < 0f;
            a = "Side";
        }
        else if (lastDirection.y < 0f)
        {
            a = "Down";
        }

        return a;
    }

    public void Play(string a)
        => Play(a, 1f);

    public void Play(string a, float speed)
    {
        anim.speed = speed;

        if (a == currentAnimation)
            return;

        currentAnimation = a;
        anim.Play(a);
    }
}
