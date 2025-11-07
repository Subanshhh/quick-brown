using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class WallWalkingCharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 50f;
    public float airAcceleration = 30f;

    [Header("Jump")]
    public float jumpForce = 13f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 3f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    [Header("Gravity")]
    public Vector2 gravityDirection = Vector2.down;
    public float gravityStrength = 30f;

    [Header("Ground Detection")]
    public LayerMask groundLayer;

    [Header("References")]
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Transform playerTransform;

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
        playerTransform = transform;
    }

    void Update()
    {
        HandleInput();
        HandleJump();
        HandleDash();
        ApplyBetterJumpPhysics();
    }

    void FixedUpdate()
    {
        ApplyGravity();

        if (isDashing) return;

        // Move perpendicular to gravity
        Vector2 moveDir = Vector2.Perpendicular(-gravityDirection).normalized;
        Vector2 targetVelocity = moveDir * moveInput * moveSpeed;

        // Maintain velocity along gravity
        Vector2 velAlongGravity = Vector2.Dot(rb.linearVelocity, gravityDirection) * gravityDirection.normalized;
        float accel = isGrounded ? acceleration : airAcceleration;

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity + velAlongGravity, accel * Time.fixedDeltaTime);
    }

    private void HandleInput()
    {
        moveInput = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1f :
                    Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;

        if (spriteRenderer)
        {
            Vector2 perp = Vector2.Perpendicular(-gravityDirection);
            if (Vector2.Dot(perp, Vector2.right * moveInput) < 0)
                spriteRenderer.flipX = true;
            else if (Vector2.Dot(perp, Vector2.right * moveInput) > 0)
                spriteRenderer.flipX = false;
        }

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
    }

    private void HandleJump()
    {
        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;

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

        if (velAlongGravity < 0) // falling
        {
            rb.linearVelocity += upDir * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (velAlongGravity > 0 && !(Keyboard.current.spaceKey.isPressed || Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed))
        {
            rb.linearVelocity += upDir * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void HandleDash()
    {
        if ((Keyboard.current.shiftKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame) &&
            Time.time >= lastDashTime + dashCooldown &&
            !isDashing)
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            lastDashTime = Time.time;

            Vector2 moveAxis = Vector2.Perpendicular(-gravityDirection).normalized;
            float dashDir = moveInput != 0 ? moveInput : 1f;
            rb.linearVelocity = moveAxis * dashDir * dashSpeed;
        }

        if (isDashing && Time.time >= dashEndTime)
        {
            isDashing = false;
        }
    }

    private void ApplyGravity()
    {
        rb.AddForce(gravityDirection.normalized * gravityStrength);
    }

    #region Collision-based Ground Detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckGround(collision, true);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckGround(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
            isGrounded = false;
    }

    private void CheckGround(Collision2D collision, bool grounded)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Dot(contact.normal, -gravityDirection) > 0.3f) // threshold for floor/wall/ceiling
                {
                    isGrounded = grounded;
                    return;
                }
            }
        }
    }
    #endregion

    // Call this to change gravity (e.g., portal)
    public void SetGravity(Vector2 newGravity)
    {
        gravityDirection = newGravity.normalized;
        float angle = Mathf.Atan2(gravityDirection.y, gravityDirection.x) * Mathf.Rad2Deg - 90f;
        playerTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
