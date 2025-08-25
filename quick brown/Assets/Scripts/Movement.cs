using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 50f;
    public float airAcceleration = 30f;

    [Header("Jumping")]
    public float jumpForce = 13f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Dashing")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;
    public LayerMask wallLayer;
    public float dashCheckDistance = 1f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool isDashing;
    private float dashEndTime;
    private float lastDashTime = -Mathf.Infinity;
    private bool jumpPressed;
    public bool isUpsideDown = false;
   


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input
        moveInput = 0f;
        if (Keyboard.current.aKey.isPressed) moveInput = -1f; //GetComponent<Transform>().localScale.x = .25f;
        if (Keyboard.current.dKey.isPressed) moveInput = 1f; //GetComponent<Transform>().localScale.x = -.25f;

        if (Mathf.Abs(moveInput) > 0.1f) GetComponent<Animator>().SetBool("isWalking",true);
        else GetComponent<Animator>().SetBool("isWalking", false);


        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            float jumpDirection = isUpsideDown ? -1f : 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpDirection);
        }

        // Dash
        if (Mouse.current.rightButton.wasPressedThisFrame && Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            float dashDir = moveInput != 0 ? moveInput : (transform.localScale.x >= 0 ? 1 : -1);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * dashDir, dashCheckDistance, wallLayer);

            if (hit.collider == null)
            {
                isDashing = true;
                dashEndTime = Time.time + dashDuration;
                rb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);
                lastDashTime = Time.time;
            }
        }

        // End dash
        if (isDashing && Time.time >= dashEndTime)
        {
            isDashing = false;
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return; // Don't move while dashing

        float targetSpeed = moveInput * moveSpeed;
        float speedDifference = targetSpeed - rb.linearVelocity.x;

        float accelRate = isGrounded ? acceleration : airAcceleration;

        float movement = speedDifference * accelRate * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);
    }
}
