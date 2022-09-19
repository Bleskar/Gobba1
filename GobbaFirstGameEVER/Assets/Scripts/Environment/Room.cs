using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2 roomSize;
    public Door[] doors = new Door[0];
    public bool extra;
    public bool locked;

    private void OnDrawGizmos()
    {
        Gizmos.color = locked ? Color.red :
            (extra ? Color.green : Color.magenta);

        Gizmos.DrawWireCube(transform.position, roomSize);

        for (int i = 0; i < doors.Length; i++)
        {
            Gizmos.color = doors[i].connection ? Color.green : Color.white;

            Gizmos.DrawWireSphere(doors[i].transform.position, .2f);
            Gizmos.DrawRay(doors[i].transform.position, doors[i].transform.forward);
        }
    }
}
