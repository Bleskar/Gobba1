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

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer);
    }

    [SerializeField] int level;
    public int CurrentLevel => level;
    public int Rooms => level + 3;
    public int ExtraRooms => level / 2;

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

    public void NextLevel()
    {
        level++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetLevels()
    {
        level = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
