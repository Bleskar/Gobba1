using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    Animator anim;
    public string CurrentAnimation { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Play(string animation)
    {
        if (animation == CurrentAnimation || !anim)
            return;

        CurrentAnimation = animation;
        anim.Play(animation);
    }
}
