using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Room parentRoom;
    public Door connection;

    [Header("Refrences")]
    [SerializeField] SpriteRenderer doorRenderer;

    private void Start()
    {
        doorRenderer.gameObject.SetActive(connection);  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!connection || !collision.GetComponent<PlayerMovement>())
            return;

        CameraController.Instance.StartRoomTransition(this, connection);
    }
}
