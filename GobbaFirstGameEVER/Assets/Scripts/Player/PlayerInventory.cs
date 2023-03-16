using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] Weapon holding;
    public Weapon Holding
    {
        get => holding;
        set => holding = value;
    }

    [SerializeField] List<StatItem> items = new List<StatItem>(0);
    public List<StatItem> Items => items;
    [SerializeField] int maxItems = 10;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.LoadStats(this);
    }

    public void DropItem(int index)
    {
        GameManager.Instance.SpawnItem(transform.position + (Vector3)Random.insideUnitCircle, items[index], PlayerMovement.Instance.CurrentRoom);
        items.RemoveAt(index);
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

    public void EquipWeapon(Weapon w)
    {
        holding = w;

        Title.Activate(w.name, w.Description);
    }

    public void AddWeaponDirect(Weapon w)
    {
        holding = w;
    }

    public void EquipItem(StatItem s)
    {
        items.Add(s);

        if (s.healthBoost > 0)
            PlayerCombat.Instance.Health += s.healthBoost;

        Title.Activate($"Picked up {s.name}");
    }

    public void AddItemDirect(StatItem s)
    {
        items.Add(s);
    }
}
