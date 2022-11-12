using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    Camera cam;
    [SerializeField] float speed;
    [SerializeField] float transitionTime = .5f;
    [SerializeField] Vector2 cameraPan = Vector2.one;

    PlayerMovement player;

    public bool Frozen => transitioning || transitionCooldown > 0f;
    private bool transitioning;
    float transitionCooldown;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = PlayerMovement.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (transitionCooldown > 0f)
            transitionCooldown -= Time.deltaTime;

        if (Frozen)
            return;

        Vector3 target = player.transform.position;
        target.z = transform.position.z;

        target += new Vector3(Input.GetAxis("Horizontal") * cameraPan.x, Input.GetAxis("Vertical") * cameraPan.y);

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 5f);
    }

    public void StartRoomTransition(Door start, Door end)
    {
        if (Frozen)
            return;

        StartCoroutine(TransitionRoom(start, end));
    }

    IEnumerator TransitionRoom(Door start, Door end)
    {
        transitioning = true;
        end.parentRoom.gameObject.SetActive(true);
        player.roomTransition = true;

        float timer = transitionTime;
        while (timer > 0f)
        {
            Vector3 target = Vector3.Lerp(start.transform.position, end.transform.position, 1f - (timer / transitionTime));
            target.z = transform.position.z;

            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 5f);

            timer -= Time.deltaTime;
            yield return null;
        }

        player.transform.position = new Vector3(end.transform.position.x, end.transform.position.y, player.transform.position.z) - end.transform.right * 2f;
        player.roomTransition = false;
        start.parentRoom.gameObject.SetActive(false);
        transitioning = false;
        transitionCooldown = .1f;
    }
}
