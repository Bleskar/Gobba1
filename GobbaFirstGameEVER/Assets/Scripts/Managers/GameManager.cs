using UnityEngine.SceneManagement;
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

        startLevel = true;

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer);
    }

    [SerializeField] int level;
    public int CurrentLevel => level;
    public int Rooms => level + 3;
    public int ExtraRooms => level / 2;

    [Header("Items")]
    [SerializeField] ItemWorld itemPrefab;
    [SerializeField] Heart heartPrefab;

    [Header("Layers")]
    [SerializeField] int enemyLayer;

    [Header("Player save data")]
    bool startLevel = true;
    [SerializeField] int health;
    [SerializeField] Weapon holding;
    [SerializeField] StatItem[] inventory = new StatItem[0];

    public void SaveStats(PlayerInventory player)
    {
        health = PlayerCombat.Instance.Health;
        holding = player.Holding;
        inventory = player.Items.ToArray();
    }

    public void LoadStats(PlayerInventory player)
    {
        if (startLevel)
            return;


        PlayerCombat.Instance.Health = health;
        player.AddWeaponDirect(holding);

        for (int i = 0; i < inventory.Length; i++)
        {
            player.AddItemDirect(inventory[i]);
        }
    }

    public ItemWorld SpawnItem(Vector3 position, ItemBase item, Room parentRoom)
    {
        ItemWorld clone = Instantiate(itemPrefab, position, Quaternion.identity);
        clone.item = item;
        clone.transform.parent = parentRoom.transform;
        return clone;
    }

    public Heart DropHeart(Vector3 position, Room parentRoom)
    {
        Heart clone = Instantiate(heartPrefab, position, Quaternion.identity);
        clone.transform.parent = parentRoom.transform;

        return clone;
    }

    public void NextLevel()
    {
        SaveStats(PlayerCombat.Instance.Inventory);
        level++;
        startLevel = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetLevels()
    {
        level = 1;
        startLevel = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
