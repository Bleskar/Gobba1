using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public int SpecialRooms => level <= 1 ? 0 : (level + 2) / 3;

    [Header("Items")]
    [SerializeField] ItemWorld itemPrefab;
    [SerializeField] Heart heartPrefab;
    [SerializeField] RatTrap trap;
    public CombineRecipe[] recipes = new CombineRecipe[0];

    [Header("Layers")]
    [SerializeField] int enemyLayer;

    [Header("Player save data")]
    bool startLevel = true;
    [SerializeField] int health;
    [SerializeField] Weapon holding;
    [SerializeField] StatItem[] inventory = new StatItem[0];
    public int enemiesKilled; 

    [Header("Particles")]
    [SerializeField] ParticleSystem[] particles = new ParticleSystem[0];

    public void PlayParticles(string particleName, Vector3 position, int emit)
    {
        ParticleSystem p = Array.Find(particles, i => i.gameObject.name == particleName);
        if (!p)
        {
            Debug.LogError($"Particles by the name of '{particleName}' doesn't exist!");
            return;
        }

        ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams
        {
            position = position,
            applyShapeToPosition = true
        };

        p.Emit(ep, emit);
    }

    public void SpawnTrap(Vector3 position)
    {
        Instantiate(trap, position, Quaternion.identity);
    }

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
        enemiesKilled = 0;
        level = 1;
        startLevel = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int Score
    {
        get
        {
            int s = (CurrentLevel - 1) * 20;
            s += enemiesKilled;
            s += PlayerMovement.Instance.Inventory.Items.Count * 3;
            return s;
        }
    }
}
