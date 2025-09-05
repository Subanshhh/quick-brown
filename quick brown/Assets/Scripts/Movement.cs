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
    public float dashCheckDistance = 1f;
    public LayerMask wallLayer;

    [Header("Misc")]
    public bool isUpsideDown = false;

    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private Transform playerTransform;
    private TrailRenderer dashTrail;


    // State
    private float moveInput;
    private bool isGrounded;
    private bool isDashing;
    private float dashEndTime;
    private float lastDashTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerTransform = transform;
        dashTrail = GetComponent<TrailRenderer>();
        dashTrail.emitting = false;

    }

    void Update()
    {
        HandleInput();
        CheckGrounded();
        HandleJump();
        HandleDash();
        HandleAnimations();
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        float targetSpeed = moveInput * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accel = isGrounded ? acceleration : airAcceleration;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x + speedDiff * accel * Time.fixedDeltaTime,
            rb.linearVelocity.y
        );
    }

    private void HandleInput()
    {
        moveInput = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput = -1f;
            playerTransform.localScale = new Vector3(1f, playerTransform.localScale.y, 1f);
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1f;
            playerTransform.localScale = new Vector3(-1f, playerTransform.localScale.y, 1f);
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleJump()
    {
        if ((Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame) && isGrounded)
        {
            float jumpDirection = isUpsideDown ? -1f : 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpDirection);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && ((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            dashTrail.emitting = false;
            isDashing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void HandleDash()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame &&
            Time.time >= lastDashTime + dashCooldown &&
            !isDashing)
        {
            float dashDir = moveInput != 0 ? moveInput : (playerTransform.localScale.x > 0 ? -1 : 1);

            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            rb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);
            lastDashTime = Time.time;
            dashTrail.emitting = true; // enable trail

        }

        if (isDashing && Time.time >= dashEndTime)
        {
            isDashing = false;
            dashTrail.emitting = false;
        }
    }

    private void HandleAnimations()
    {
        if (animator == null) return;

        bool isWalking = Mathf.Abs(moveInput) > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
