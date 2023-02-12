using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] Weapon holding;
    public Weapon Holding => holding;

    [SerializeField] List<StatItem> items = new List<StatItem>(0);
    public List<StatItem> Items => items;
    [SerializeField] int maxItems = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpItem(ItemWorld item)
    {
        if (item.item.GetType() == typeof(Weapon))
        {
            Weapon temp = (Weapon)item.item;

            if (Holding)
            {
                item.item = Holding;
                item.ItemSlide();
            }
            else
            {
                Destroy(item.gameObject);
            }

            EquipWeapon(temp);

            item.ItemSlide();
        }
        else if (items.Count < maxItems && item.item.GetType() == typeof(StatItem))
        {
            EquipItem((StatItem)item.item);
            Destroy(item.gameObject);
        }
    }

    void EquipWeapon(Weapon w)
    {
        holding = w;

        Title.Activate(w.name, w.description);
    }

    void EquipItem(StatItem s)
    {
        items.Add(s);

        Title.Activate(s.name, s.description);
    }
}
