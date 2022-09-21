using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    [Header("Basic information")]
    public new string name = "New Item";
    public Sprite coverImage;
    [TextArea(3, 4)] public string description;
}