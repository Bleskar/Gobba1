using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer);
    }

    [Header("Item")]
    [SerializeField] ItemWorld itemPrefab;

    [Header("Layers")]
    [SerializeField] int enemyLayer;

    public ItemWorld SpawnItem(Vector3 position, ItemBase item, Room parentRoom)
    {
        ItemWorld clone = Instantiate(itemPrefab, position, Quaternion.identity);
        clone.item = item;
        clone.transform.parent = parentRoom.transform;
        return clone;
    }
}
