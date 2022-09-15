using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemBase : ScriptableObject
{
    [Header("Basic information")]
    public new string name = "New Item";
    public Sprite coverImage;
}