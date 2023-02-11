using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] Image item;

    private void Update()
    {
        item.sprite = PlayerCombat.Instance.Holding.coverImage;
    }
}
