using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Room parentRoom;
    public Door connection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!connection)
            return;

        CameraController.Instance.StartRoomTransition(this, connection);
    }
}
