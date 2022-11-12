using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropTable<T>
{
    public Item[] items = new Item[0];

    float maxRandom = 0f;
    public void Initialize()
    {
        maxRandom = 0f;
        for (int i = 0; i < items.Length; i++)
        {
            items[i].min = maxRandom;
            maxRandom += items[i].weight;
        }
    }

    public T Drop()
    {
        float r = Random.value * maxRandom;
        return System.Array.Find(items, i => r >= i.min && r <= i.Max).item;
    }

    [System.Serializable]
    public struct Item
    {
        public T item;
        public float weight;

        [HideInInspector] public float min;
        public float Max => min + weight;
    }
}
