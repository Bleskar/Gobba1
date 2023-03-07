using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite open;
    public ItemBase item;
    public Room room;
    bool chestOpened = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (chestOpened || !collision.GetComponent<PlayerMovement>())
        {
            return;
        }
        chestOpened = true;
        ItemWorld itm = GameManager.Instance.SpawnItem(transform.position, item, room);
        itm.gameObject.transform.parent = this.transform;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = open;
    }
}
