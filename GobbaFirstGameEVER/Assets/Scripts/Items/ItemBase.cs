using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    [Header("Basic information")]
    public new string name = "New Item";
    public Sprite coverImage;
    [SerializeField] [TextArea(3, 4)] string description;
    public virtual string Description => description;
}