using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] GraphicRaycaster raycaster;
    [SerializeField] RectTransform tooltip;
    [SerializeField] Text title;
    [SerializeField] Text description;

    // Update is called once per frame
    void Update()
    {
        StatItem item = null;

        //Check hover
        InventorySlotUI slot = CheckHoverItemSlot();
        if (slot)
            item = PlayerMovement.Instance.Inventory.Items[slot.index];
        else
        {
            ProductSlot ps = CheckHoverProduct();

            if (ps && ps.item)
            {
                item = ps.item.GetType() == typeof(StatItem) ? ((StatItem)ps.item) : null;
            }
            else
                item = CheckHover();
        }

        //Display information
        if (!item)
        {
            tooltip.gameObject.SetActive(false);
        }
        else
        {
            ShowTooltip(item);
            tooltip.position = Input.mousePosition - (Vector3)tooltip.sizeDelta * .5f;

            if (Input.GetKeyDown(KeyCode.Q) && slot)
                DropItem(slot.index);
            else if (Input.GetMouseButtonDown(0) && slot && AnvilMenu.Instance.gameObject.activeSelf)
                AnvilMenu.Instance.SelectItem(slot.index);
        }    
    }

    void DropItem(int index)
    {
        PlayerMovement.Instance.Inventory.DropItem(index);
    }

    void ShowTooltip(StatItem item)
    {
        tooltip.gameObject.SetActive(true);

        title.text = item.name;
        string desc = item.Description.Trim() != "" ? $"{item.Description}\n" : "";

        desc += StatDisplay("Attack boost", item.atkBoost);
        desc += StatDisplay("Attack speed", item.atkspd);
        desc += StatDisplay("Health boost", item.healthBoost);
        desc += StatDisplay("Speed boost", item.speedBoost);
        desc += StatDisplay("Dash speed boost", item.dashBoost);

        description.text = desc;
    }

    string StatDisplay(string header, float var)
    {
        if (var == 0)
            return "";

        return $"{header} {(var > 0f ? "+" : "")}{var:N0}\n";
    }

    InventorySlotUI CheckHoverItemSlot()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> res = new List<RaycastResult>(0);

        raycaster.Raycast(eventData, res);

        foreach (RaycastResult item in res)
        {
            InventorySlotUI slot = item.gameObject.GetComponent<InventorySlotUI>();
            if (slot)
                return slot;
        }

        return null;
    }

    public ProductSlot CheckHoverProduct()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> res = new List<RaycastResult>(0);

        raycaster.Raycast(eventData, res);

        foreach (RaycastResult item in res)
        {
            ProductSlot slot = item.gameObject.GetComponent<ProductSlot>();
            if (slot)
                return slot;
        }

        return null;
    }

    public StatItem CheckHover()
    {
        Collider2D[] cda = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        for (int i = 0; i < cda.Length; i++)
        {
            ItemWorld item = cda[i].GetComponent<ItemWorld>();
            if (item && item.item.GetType() == typeof(StatItem)) return (StatItem)item.item;
        }
        return null;
    }
}
