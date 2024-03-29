using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [SerializeField] Weapon holding;
    public Weapon Holding
    {
        get => holding;
        set => holding = value;
    }

    [SerializeField] List<StatItem> items = new List<StatItem>(0);
    public List<StatItem> Items => items;
    [SerializeField] int maxItems = 10;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.LoadStats(this);
    }

    public void RemoveItem(int index)
    {
        items.RemoveAt(index);
    }

    public void DropItem(int index)
    {
        //GameManager.Instance.SpawnItem(transform.position + (Vector3)Random.insideUnitCircle, items[index], PlayerMovement.Instance.CurrentRoom);
        items.RemoveAt(index);
    }

    public bool HasPerk(ItemSpecial special) =>
        PerkLevel(special) > 0;

    public int PerkLevel(ItemSpecial special)
    {
        int level = 0;
        foreach (StatItem item in items)
        {
            if (item.special == special)
                level++;
        }
        return level;
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
