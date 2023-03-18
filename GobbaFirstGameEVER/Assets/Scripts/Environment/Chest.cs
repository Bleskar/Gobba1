using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    SpriteRenderer sr;
    public Sprite open;
    public ItemBase item;
    public Room room;
    bool chestOpened = false;
    public Vector3 offset;

    public void Interact()
    {
        if (chestOpened)
        {
            return;
        }

        AudioManager.Play("ChestOpen");
        chestOpened = true;
        ItemWorld itm = GameManager.Instance.SpawnItem(transform.position + offset, item, room);
        itm.gameObject.transform.parent = this.transform;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = open;
    }
}
