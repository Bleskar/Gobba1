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
    [SerializeField] Transform[] spawnPositions = new Transform[0];
    [SerializeField] int minNumberOfMobs = 0;
    [SerializeField] int maxNumberOfMobs = 1;

    [Header("Destructable Objects")]
    [SerializeField] int maxRemoveObjects = 2;
    [SerializeField] int minRemoveObjects = 1;
    [SerializeField] Transform objectsParent;

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

        Gizmos.color = Color.red;
        foreach (Transform p in spawnPositions)
        {
            if (!p)
                continue;

            Gizmos.DrawSphere(p.transform.position, .2f);
        }
    }

    void SpawnEnemies()
    {
        //ser till att ni inte gör något fel här (jag tittar på dig Filip Ryefalk)
        if (maxNumberOfMobs > spawnPositions.Length)
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
            Transform[] availablePositions = System.Array.FindAll(spawnPositions, j => j != null);
            Transform selectedSpawnPos = availablePositions[Random.Range(0, availablePositions.Length)];

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
