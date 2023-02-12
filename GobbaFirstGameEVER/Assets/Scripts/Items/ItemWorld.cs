using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public ItemBase item;

    [Header("Movement")]
    [SerializeField] float friction = 60f;
    [SerializeField] float slideSpeed = 20f;

    SpriteRenderer sr;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.sprite = item.coverImage;

        Vector2 dir = rb.velocity.normalized;
        rb.velocity -= dir * friction * Time.deltaTime; // item friction
        if (rb.velocity.normalized != dir)
            rb.velocity = Vector2.zero; // items stops sliding
    }

    public void ItemSlide()
    {
        rb.velocity = Random.insideUnitCircle.normalized * slideSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInventory pi = collision.GetComponent<PlayerInventory>();
        if (pi)
        {
            pi.PickUpItem(this);
        }
    }
}
