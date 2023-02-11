using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Room parentRoom;
    public Door connection;
    public bool bossDoor;

    [Header("Refrences")]
    [SerializeField] SpriteRenderer doorRenderer;

    private void Start()
    {
        doorRenderer.gameObject.SetActive(connection);  
    }

    private void Update()
    {
        doorRenderer.color = bossDoor ? Color.red : Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!connection || !collision.GetComponent<PlayerMovement>())
            return;

        CameraController.Instance.StartRoomTransition(this, connection);
    }
}
