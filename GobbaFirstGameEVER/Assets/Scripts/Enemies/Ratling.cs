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
        sr = GetComponent<SpriteRenderer>();
        deathTimer = 20f;
        float dmg = damage;
        dmg *= Mathf.Exp((GameManager.Instance.CurrentLevel - 1) / 4);
        damage = (int)Mathf.Round(dmg);
        StartCoroutine(StartUp());
    }

    void Update()
    {
        sr.enabled = !dead || alwaysShow;

        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
        {
            Kill(null);
        }

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
