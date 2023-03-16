using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Image[] inventorySprites = new Image[0];
    [SerializeField] InventorySlotUI[] slots = new InventorySlotUI[0];
    PlayerInventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = PlayerMovement.Instance.Inventory;
        slots = new InventorySlotUI[inventorySprites.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = inventorySprites[i].gameObject.AddComponent<InventorySlotUI>();
            slots[i].parent = this;
            slots[i].index = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < inventorySprites.Length; i++)
        {
            inventorySprites[i].enabled = i < inventory.Items.Count;
            if (inventorySprites[i].enabled)
            {
                //item exists in this slot
                inventorySprites[i].sprite = inventory.Items[i].coverImage;
            }
        }
    }
}
