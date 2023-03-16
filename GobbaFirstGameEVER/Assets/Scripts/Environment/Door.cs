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

    public float delay = 1f;
    float time;
    private void Start()
    {
        time = 1f;
        doorRenderer.gameObject.SetActive(connection);

    }

    private void Update()
    {
        if (!parentRoom.enemiesKilled && connection)
        {
            if (time <= 0f)
            {
                doorRenderer.gameObject.SetActive(false);

            }
            else
            {
                time -= Time.deltaTime;
            }
        }
        else
        {
            doorRenderer.gameObject.SetActive(connection);
            SpriteRenderer sr = doorRenderer.gameObject.GetComponentInChildren<SpriteRenderer>();
            if (sr)
            {
                sr.color = bossDoor ? Color.red : Color.white;
            }
            else
            {
                doorRenderer.color = bossDoor ? Color.red : Color.white;
            }
            

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!connection || !collision.GetComponent<PlayerMovement>() || !parentRoom.enemiesKilled)
            return;

        CameraController.Instance.StartRoomTransition(this, connection);
    }
}
