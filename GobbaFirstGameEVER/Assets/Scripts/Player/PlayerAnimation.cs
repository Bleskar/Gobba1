using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    SpriteRenderer sr;
    
    Animator anim;
    string currentAnimation;

    Vector2 lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Play(GetNormalAnimation());
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

        if (lastDirection.x != 0f)
        {
            sr.flipX = lastDirection.x < 0f;
            a = "Side" + a;
        }
        else if (lastDirection.y < 0f)
        {
            a = "Down" + a;
        }
        else
        {
            a = "Up" + a;
        }

        return a;
    }

    void Play(string a)
    {
        if (a == currentAnimation)
            return;

        currentAnimation = a;
        anim.Play(a);
    }
}
