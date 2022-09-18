using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Options")]
    [SerializeField] float acceleration = 50f;
    [SerializeField] float deacceleration = 50f;
    [SerializeField] float topSpeed = 8f;

    [Header("Dash")]
    [SerializeField] float dashLength = 5f;
    [SerializeField] float dashSpeed = 20f;
    float DashTime => dashLength / dashSpeed;
    public bool Dashing { get; private set; }

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
            if (rb.velocity.magnitude > topSpeed)
                rb.velocity = rb.velocity.normalized * topSpeed;
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
        enabled = false;
        Dashing = true;

        float timer = 0f;
        while (timer < DashTime)
        {
            rb.velocity = direction * dashSpeed;

            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = direction * topSpeed;

        enabled = true;
        Dashing = false;
    }
}
