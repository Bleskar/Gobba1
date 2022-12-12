using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2 roomSize;
    public Door[] doors = new Door[0];
    public bool extra;
    public bool locked;

    [Header("Enemies")]
    [SerializeField] DropTable<EnemyBase> spawnableMobs = new DropTable<EnemyBase>();
    [SerializeField] Transform spawnPositionsParent;
    [SerializeField] int minNumberOfMobs = 0;
    [SerializeField] int maxNumberOfMobs = 1;

    [Header("Destructable Objects")]
    [SerializeField] int maxRemoveObjects = 2;
    [SerializeField] int minRemoveObjects = 1;
    [SerializeField] Transform objectsParent;

    [Header("Items")]
    [SerializeField] [Range(0f, 1f)] float dropChance;
    [SerializeField] DropTable<ItemBase> itemDrops = new DropTable<ItemBase>();
    [SerializeField] Transform dropArea;
    [SerializeField] ItemWorld itemPrefab;
    bool enemiesKilled;

    //current mobs and breakable objects
    List<GameObject> currentBreakableObjects = new List<GameObject>();
    List<EnemyBase> currentEnemies = new List<EnemyBase>();
    
    private void Awake()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].parentRoom = this;
        }
    }

    private void Start()
    {
        if (this != PlayerMovement.Instance.CurrentRoom)
            gameObject.SetActive(false);

        SpawnEnemies();
        RemoveDestructableObjects();
    }

    private void Update()
    {
        if (!enemiesKilled && currentEnemies.Count <= 0)
        {
            enemiesKilled = true;
            RoomCleared();
        }
    }

    void RoomCleared()
    {
        float dc = Random.Range(0f, 1f);
        if (dc < dropChance)
            DropTreasure();
    }

    void DropTreasure()
    {
        ItemBase drop = itemDrops.Drop();
        ItemWorld clone = Instantiate(itemPrefab, dropArea.position, Quaternion.identity);
        clone.item = drop;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = locked ? Color.red :
            (extra ? Color.green : Color.magenta);

        Gizmos.DrawWireCube(transform.position, roomSize);

        for (int i = 0; i < doors.Length; i++)
        {
            Gizmos.color = doors[i].connection ? Color.green : Color.white;

            Gizmos.DrawWireSphere(doors[i].transform.position, .2f);
            Gizmos.DrawRay(doors[i].transform.position, doors[i].transform.right);
        }

        if (!spawnPositionsParent)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < spawnPositionsParent.childCount; i++)
        {
            Gizmos.DrawSphere(spawnPositionsParent.GetChild(i).position, .2f);
        }
    }

    void SpawnEnemies()
    {
        //ser till att ni inte gör något fel här (jag tittar på dig Filip Ryefalk)
        if (maxNumberOfMobs > spawnPositionsParent.childCount)
        {
            Debug.LogError($"Hallå! maxNumberOfMobs får inte vara större än antalet spawn-positioner! (Room Name: {gameObject.name})");
            return;
        }
        if (minNumberOfMobs < 0)
        {
            Debug.LogError($"Hallå! minNumberOfMobs får inte vara mindre än 0! (Room Name: {gameObject.name})");
            return;
        }

        Transform enemyParent = new GameObject("Enemy Parent").transform;
        enemyParent.parent = transform;

        for (int i = minNumberOfMobs; i < maxNumberOfMobs; i++)
        {
            //randomizes a position for the new enemy to be placed at
            //then removes the spawnposition so enemies cant spawn on top of each other
            Transform selectedSpawnPos = spawnPositionsParent.GetChild(Random.Range(0, spawnPositionsParent.childCount));

            //creates the enemy
            EnemyBase clone = Instantiate(spawnableMobs.Drop(), selectedSpawnPos.position, Quaternion.identity);
            clone.startPosition = selectedSpawnPos.position;
            currentEnemies.Add(clone);
            clone.transform.parent = enemyParent;

            Destroy(selectedSpawnPos.gameObject); //destroys spawnpos after its used
        }
    }

    void RemoveDestructableObjects()
    {
        int remove = Random.Range(minRemoveObjects, maxRemoveObjects + 1);
        for (int i = 0; i < remove; i++)
        {
            //destroy objects children (i.e. the destructable objects)
            GameObject o = objectsParent.GetChild(Random.Range(0, objectsParent.childCount)).gameObject;
            Destroy(o);
        }
    }

    private void OnEnable()
    {
        PlayerEnterRoom();
    }

    public void PlayerEnterRoom()
    {
        foreach (EnemyBase e in currentEnemies)
        {
            e.RoomEnter();
        }
    }
}
