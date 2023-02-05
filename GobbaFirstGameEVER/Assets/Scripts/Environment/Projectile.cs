using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask targetLayers;

    public Vector3 direction;
    public float speed = 20f;
    public int damage;

    public bool Travelling => travelling && !dead;

    bool dead;
    public bool travelling;

    // Update is called once per frame
    void Update()
    {
        if (Travelling)
            Travel();
    }

    public void Travel()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, speed * Time.deltaTime, targetLayers);
        if (hit)
        {
            transform.position = hit.point;
            Kill(hit);
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void Kill(RaycastHit2D hit)
    {
        PlayerCombat pc = hit.transform.GetComponent<PlayerCombat>();
        if (pc)
            pc.Damage(damage, direction);

        Destroy(gameObject);
    }
}
