using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratling : Projectile
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(StartUp());
    }

    void Update()
    {
        if (Travelling)
        {
            transform.right = direction;
            Travel();
        }
    }

    IEnumerator StartUp()
    {
        anim.Play("Start");
        travelling = false;

        yield return new WaitForSeconds(.5f);

        anim.Play("Shoot");
        travelling = true;

        direction = ((Vector2)PlayerMovement.Instance.transform.position - (Vector2)transform.position).normalized;
    }
}
