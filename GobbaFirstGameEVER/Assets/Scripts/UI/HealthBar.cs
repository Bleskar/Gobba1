using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] RectTransform bar;
    [SerializeField] RectTransform parent;
    [SerializeField] Text count;

    // Update is called once per frame
    void Update()
    {
        bar.sizeDelta = new Vector2(((float)PlayerCombat.Instance.Health / PlayerCombat.Instance.MaxHealth) * (parent.sizeDelta.x - 11f), 100f);
        bar.anchoredPosition = Vector2.right * (6.5f + bar.sizeDelta.x * .5f);
        count.text = $"{PlayerCombat.Instance.Health}/{PlayerCombat.Instance.MaxHealth}";
    }
}
