using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New recipe", menuName = "Combine recipe")]
public class CombineRecipe : ScriptableObject
{
    public ItemBase item1;
    public ItemBase item2;
    public ItemBase product;
}
