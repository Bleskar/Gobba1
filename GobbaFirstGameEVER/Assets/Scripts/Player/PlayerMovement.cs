using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    public Room CurrentRoom { get; private set; }

    [Header("Movement Options")]
    [SerializeField] float acceleration = 50f;
    [SerializeField] float deacceleration = 50f;
    [SerializeField] float topSpeed = 8f;

    float TopSpeed
    {
        get
        {
            float s = topSpeed;

            foreach (StatItem i in Inventory.Items)
            {
                s += i.speedBoost;
            }

            return s;
        }
    }

    float DashSpeed
    {
        get
        {
            float s = dashSpeed;

            foreach (StatItem i in Inventory.Items)
            {
                s += i.dashBoost;
            }

            return s;
        }
    }

    [Header("Dash")]
    [SerializeField] float dashLength = 5f;
    [SerializeField] float dashSpeed = 20f;
    float DashTime => dashLength / DashSpeed;
    public bool Dashing { get; private set; }

    public bool Frozen => roomTransition || Dashing || PlayerCombat.Instance.Dead || PlayerCombat.Instance.Damaged;
    public bool roomTransition;

    Rigidbody2D rb;
    SpriteRenderer sr;

    private void Awake()
    {
        Inventory = GetComponent<PlayerInventory>();
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Title.Activate($"Level {GameManager.Instance.CurrentLevel}");
    }

    // Update is called once per frame
    void Update()
    {
        sr.enabled = !roomTransition;

        if (Frozen)
            return;

        if (Input.GetButtonDown("Dash"))
        {
            StartCoroutine(
                Dash(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))));
            return;
        }

        Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public void Movement(float x, float y) => Movement(new Vector2(x, y));
    public void Movement(Vector2 input)
    {
        input.Normalize();

        if (input != Vector2.zero)
        {
            //moving
            rb.velocity += input * Time.deltaTime * acceleration;
            if (rb.velocity.magnitude > TopSpeed)
                rb.velocity = rb.velocity.normalized * TopSpeed;
            return;
        }
        //breaking
        Vector2 direction = rb.velocity.normalized;
        rb.velocity -= direction * Time.deltaTime * deacceleration;

        if (rb.velocity.normalized != direction)
            rb.velocity = Vector2.zero;
    }

    IEnumerator Dash(Vector2 direction)
    {
        AudioManager.Play("Dash");
        Dashing = true;

        float timer = 0f;
        while (timer < DashTime)
        {
            rb.velocity = direction * DashSpeed;

            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = direction * TopSpeed;

        Dashing = false;
    }

    public void SetRoom(Room room)
    {
        if (!room)
            return;

        CurrentRoom?.gameObject.SetActive(false);
        CurrentRoom = room;
        room.gameObject.SetActive(true);
    }
}
