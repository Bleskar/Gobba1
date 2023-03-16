using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Image[] inventorySprites = new Image[0];
    PlayerInventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = PlayerMovement.Instance.Inventory;
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
