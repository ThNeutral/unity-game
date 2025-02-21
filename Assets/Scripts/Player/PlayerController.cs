using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;

    public Transform orientation;

    float horizontalinput;
    float verticalinput;

    Vector3 direction;
    Rigidbody rb;

    public float playerHeight;
    public LayerMask ground;
    public bool grounded;
    public float groundDrag;

    public float jumpforce;
    public float jumpcd;
    public float airmultiplier;
    bool jumpready;

    public KeyCode jumpKey = KeyCode.Space;

    private TowerController towerController;

    private void Start()
    {
        jumpready = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        MoveInput();
        MoveControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        
    }

    void FixedUpdate()
    {
        Move();
    }

    private void MoveInput()
    {
        horizontalinput = Input.GetAxisRaw("Horizontal");
        verticalinput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && jumpready && grounded)
        {
            jumpready = false;
            Jump();
            Invoke(nameof(JumpReset), jumpcd);
        }
    }

    private void Move()
    {
        direction = orientation.forward * verticalinput + orientation.right * horizontalinput;
        if(grounded)
            rb.AddForce(direction.normalized * Speed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(direction.normalized * Speed * 10f * airmultiplier, ForceMode.Force);
    }

    private void MoveControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > Speed)
        {
            Vector3 limitedVel = flatVel.normalized * Speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
    }

    private void JumpReset()
    {
        jumpready = true;
    }
}
