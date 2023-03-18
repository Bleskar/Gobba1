using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    [Header("Rooms")]
    [SerializeField] Room startRoom;
    [SerializeField] Room[] roomPrefabs = new Room[0];
    [SerializeField] Room[] specialRoomPrefabs = new Room[0];

    int Rooms => GameManager.Instance.Rooms;
    int ExtraRooms => GameManager.Instance.ExtraRooms;
    int SpecialRooms => GameManager.Instance.SpecialRooms;

    [SerializeField] Room bossRoom;

    List<Room> dungeon = new List<Room>();

    [Header("Items")]
    [SerializeField] [Range(0f, 1f)] float dropChance;
    [SerializeField] DropTable<ItemBase> itemDrops = new DropTable<ItemBase>();

    public void Start()
    {
        itemDrops.Initialize();
        StartGeneration();
    }

    public void StartGeneration()
    {
        //Create start room
        Room currentRoom = CreateNewRoom(Vector3.zero, startRoom);
        PlayerMovement.Instance.SetRoom(currentRoom);
        dungeon.Add(currentRoom);

        Door door;
        Door newDoor;

        #region main path
        //create main path
        for (int i = 0; i < Rooms; i++)
        {
            door = GetDoorFromRoom(currentRoom);
            Vector3 roomOffset = door.transform.localPosition;

            currentRoom = CreateRoomFromRoom(currentRoom, roomOffset);

            newDoor = GetDoorFromDoor(currentRoom, door);
            currentRoom.transform.position -= newDoor.transform.localPosition;

            door.connection = newDoor;
            newDoor.connection = door;

            dungeon.Add(currentRoom);
        }
        #endregion

        Room pivot;

        #region boss room
        //Create boss room
        pivot = GetRandomPivotFromIndex(dungeon.Count-1, dungeon.Count);

        door = GetDoorFromRoom(pivot);
        door.bossDoor = true;
        currentRoom = Instantiate(bossRoom, pivot.transform.position + door.transform.localPosition, Quaternion.identity);

        newDoor = GetDoorFromDoor(currentRoom, door);
        currentRoom.transform.position -= newDoor.transform.localPosition;

        door.connection = newDoor;
        newDoor.connection = door;

        currentRoom.locked = true;
        dungeon.Add(currentRoom);
        #endregion

        #region extra rooms
        // Create extra rooms
        for (int i = 0; i < ExtraRooms; i++)
        {
            pivot = GetRandomPivot();

            door = GetDoorFromRoom(pivot);
            Vector3 roomOffset = door.transform.localPosition;

            currentRoom = CreateRoomFromRoom(pivot, roomOffset);

            newDoor = GetDoorFromDoor(currentRoom, door);
            currentRoom.transform.position -= newDoor.transform.localPosition;

            door.connection = newDoor;
            newDoor.connection = door;

            currentRoom.extra = true;
            dungeon.Add(currentRoom);
        }
        #endregion

        #region special rooms
        // Create special rooms
        for (int i = 0; i < SpecialRooms; i++)
        {
            pivot = GetRandomPivot();

            door = GetDoorFromRoom(pivot);
            Vector3 roomOffset = door.transform.localPosition;

            currentRoom = CreateSpecialRoomFromRoom(pivot, roomOffset);

            newDoor = GetDoorFromDoor(currentRoom, door);
            currentRoom.transform.position -= newDoor.transform.localPosition;

            door.connection = newDoor;
            newDoor.connection = door;

            currentRoom.extra = true;
            dungeon.Add(currentRoom);
        }
        #endregion
    }

    Room GetRandomPivot()
    {
        List<Room> availableRooms = dungeon.FindAll(r => HasAvailableDoor(r) && !r.locked);
        return availableRooms[Random.Range(0, availableRooms.Count)];
    }

    Room GetRandomPivotFromIndex(int min, int max)
    {
        List<Room> availableRooms = new List<Room>(0);

        for (int i = min; i < max; i++)
        {
            if (HasAvailableDoor(dungeon[i]) && !dungeon[i].locked)
                availableRooms.Add(dungeon[i]);
        }

        return availableRooms[Random.Range(0, availableRooms.Count)];
    }

    Room CreateRoomFromRoom(Room r, Vector3 offset)
    {
        return CreateNewRoom(r.transform.position + offset,
                roomPrefabs[Random.Range(0, roomPrefabs.Length)]);
    }

    Room CreateSpecialRoomFromRoom(Room r, Vector3 offset)
    {
        return CreateNewRoom(r.transform.position + offset,
                specialRoomPrefabs[Random.Range(0, specialRoomPrefabs.Length)]);
    }

    bool HasAvailableDoor(Room r)
    {
        for (int i = 0; i < r.doors.Length; i++)
        {
            if (!r.doors[i].connection)
                return true;
        }
        return false;
    }

    Room CreateNewRoom(Vector3 position, Room prefab)
    {
        Room r = Instantiate(prefab, position, Quaternion.identity);
        
        if (Random.value <= dropChance)
        {
            r.itemDrop = itemDrops.Drop();
        }

        return r;
    }

    Door GetDoorFromRoom(Room r)
    {
        Door[] availableDoors = System.Array.FindAll(r.doors, i => !i.connection);
        return availableDoors[Random.Range(0, availableDoors.Length)];
    }

    Door GetDoorFromDoor(Room r, Door door)
    {
        Door[] availableDoors = System.Array.FindAll(r.doors, i => i.transform.right == -door.transform.right);
        return availableDoors[Random.Range(0, availableDoors.Length)];
    }
}
