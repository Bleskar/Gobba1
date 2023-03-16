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
        InventorySlotUI slot = CheckHover();
        if (!slot)
        {
            tooltip.gameObject.SetActive(false);
        }
        else
        {
            ShowTooltip(slot);
            tooltip.position = Input.mousePosition - (Vector3)tooltip.sizeDelta * .5f;
        }    
    }

    void ShowTooltip(InventorySlotUI slot)
    {
        tooltip.gameObject.SetActive(true);

        StatItem item = PlayerMovement.Instance.Inventory.Items[slot.index];

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

    InventorySlotUI CheckHover()
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
}
