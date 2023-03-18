using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductSlot : MonoBehaviour
{
    public Image itemImage;
    public ItemBase item;

    private void Update()
    {
        itemImage.enabled = item;
        if (!item) return;
        itemImage.sprite = item.coverImage;
    }
}
