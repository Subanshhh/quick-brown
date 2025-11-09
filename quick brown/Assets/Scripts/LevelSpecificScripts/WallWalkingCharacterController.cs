using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class WallWalkingCharacterController : MonoBehaviour
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

    [Header("Gravity")]
    public Vector2 gravityDirection = Vector2.down;

    [Header("Ground Detection")]
    public float groundRaycastDistance = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform playerTransform;
    private TrailRenderer dashTrail;

    private float moveInput;
    private bool isGrounded;
    private bool isDashing;
    private float dashEndTime;
    private float lastDashTime = -Mathf.Infinity;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

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

        Vector2 perp = Vector2.Perpendicular(-gravityDirection).normalized;
        float targetSpeed = moveInput * moveSpeed;
        Vector2 targetVelocity = perp * targetSpeed;

        // maintain velocity along gravity
        Vector2 velAlongGravity = Vector2.Dot(rb.linearVelocity, gravityDirection) * gravityDirection.normalized;
        float accelRate = isGrounded ? acceleration : airAcceleration;

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity + velAlongGravity, accelRate * Time.fixedDeltaTime);
    }

    private void HandleInput()
    {
        moveInput = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput = 1f;

        // Flip sprite perpendicular to gravity
        if (animator)
        {
            Vector2 perp = Vector2.Perpendicular(-gravityDirection).normalized;
            if (Vector2.Dot(perp, Vector2.right * moveInput) < 0)
                animator.transform.localScale = new Vector3(-Mathf.Abs(animator.transform.localScale.x), animator.transform.localScale.y, animator.transform.localScale.z);
            else if (Vector2.Dot(perp, Vector2.right * moveInput) > 0)
                animator.transform.localScale = new Vector3(Mathf.Abs(animator.transform.localScale.x), animator.transform.localScale.y, animator.transform.localScale.z);
        }

        if (moveInput != 0) AudioManager.PlayFoxMove();
    }

    private void RaycastGroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, gravityDirection, groundRaycastDistance, groundLayer);
        Debug.DrawRay(transform.position, gravityDirection * groundRaycastDistance, Color.green);
        isGrounded = hit.collider != null;
    }

    private void HandleJump()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = -gravityDirection.normalized * jumpForce;
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
    }

    private void ApplyBetterJumpPhysics()
    {
        Vector2 upDir = -gravityDirection.normalized;
        float velAlongGravity = Vector2.Dot(rb.linearVelocity, gravityDirection);

        if (velAlongGravity < 0)
            rb.linearVelocity += upDir * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (velAlongGravity > 0 && !(Keyboard.current.spaceKey.isPressed || Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed))
            rb.linearVelocity += upDir * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    private void HandleDash()
    {
        if ((Keyboard.current.rightShiftKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame) &&
            Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            lastDashTime = Time.time;

            Vector2 dashDir = Vector2.Perpendicular(-gravityDirection).normalized * (moveInput != 0 ? moveInput : 1f);
            rb.linearVelocity = dashDir * dashSpeed;
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
        if (!animator) return;
        animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0.1f);
    }

    // Call this to set new gravity (used by portal)
    public void SetGravity(Vector2 newGravity)
    {
        gravityDirection = newGravity.normalized;
        float angle = Mathf.Atan2(gravityDirection.y, gravityDirection.x) * Mathf.Rad2Deg - 90f;
        playerTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
