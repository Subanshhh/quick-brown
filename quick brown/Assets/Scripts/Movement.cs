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

    [Header("Dashing")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    [Header("Better Jump")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 3f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    [Header("Misc")]
    public bool isUpsideDown = false;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform playerTransform;
    private TrailRenderer dashTrail;

    private float moveInput;
    [SerializeField] private bool isGrounded;
    private bool isDashing;
    private float dashEndTime;
    private float lastDashTime = -Mathf.Infinity;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    [Header("Ground Detection")]
    public float groundRaycastDistance = 0.2f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerTransform = transform;
        dashTrail = GetComponent<TrailRenderer>();
        if (dashTrail) dashTrail.emitting = false;
    }

    void Update()
    {
        HandleInput();
        RaycastGroundCheck();

        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;

        if (Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.upArrowKey.wasPressedThisFrame ||
            Keyboard.current.wKey.wasPressedThisFrame)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        HandleJump();
        HandleDash();
        HandleAnimations();
        ApplyBetterJumpPhysics();
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        float targetSpeed = moveInput * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = isGrounded ? acceleration : airAcceleration;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x + speedDiff * accelRate * Time.fixedDeltaTime,
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
            AudioManager.PlayFoxMove();
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1f;
            playerTransform.localScale = new Vector3(-1f, playerTransform.localScale.y, 1f);
            AudioManager.PlayFoxMove();
        }
    }

    private void RaycastGroundCheck()
    {
        Vector2 direction = isUpsideDown ? Vector2.up : Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, groundRaycastDistance, groundLayer);

        Debug.DrawRay(transform.position, direction * groundRaycastDistance, Color.green);

        isGrounded = hit.collider != null;
    }

    private void HandleJump()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            float jumpDir = isUpsideDown ? -1f : 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpDir);

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
    }

    private void ApplyBetterJumpPhysics()
    {
        float gravityY = Physics2D.gravity.y;

        if (!isUpsideDown)
        {
            if (rb.linearVelocity.y < 0f)
                rb.linearVelocity += Vector2.up * gravityY * (fallMultiplier - 1) * Time.deltaTime;
            else if (rb.linearVelocity.y > 0f &&
                     !(Keyboard.current.spaceKey.isPressed || Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed))
                rb.linearVelocity += Vector2.up * gravityY * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else
        {
            if (rb.linearVelocity.y > 0f)
                rb.linearVelocity += Vector2.up * -gravityY * (fallMultiplier - 1) * Time.deltaTime;
            else if (rb.linearVelocity.y < 0f &&
                     !(Keyboard.current.spaceKey.isPressed || Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed))
                rb.linearVelocity += Vector2.up * -gravityY * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void HandleDash()
    {
        if ((Mouse.current.rightButton.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame) &&
            Time.time >= lastDashTime + dashCooldown &&
            !isDashing)
        {
            float dashDir = moveInput != 0 ? moveInput : (playerTransform.localScale.x > 0 ? -1 : 1);
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            rb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);
            lastDashTime = Time.time;

            if (dashTrail) dashTrail.emitting = true;
        }

        if (isDashing && Time.time >= dashEndTime)
        {
            isDashing = false;
            if (dashTrail) dashTrail.emitting = false;
        }
    }

    private void HandleAnimations()
    {
        if (animator == null) return;
        animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0.1f);
    }
}
