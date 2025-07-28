using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 15f;
    public float dashCooldown = 1f;
    public float dashCheckDistance = 1f;
    public LayerMask wallLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool isUpsideDown = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastDashTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move left/right
        float move = 0f;
        if (Keyboard.current.aKey.isPressed)
            move = -1f;
        else if (Keyboard.current.dKey.isPressed)
            move = 1f;

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            float jumpDirection = isUpsideDown ? -1 : 1;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpDirection);
        }

        // Dash
        if (Mouse.current.rightButton.wasPressedThisFrame && Time.time >= lastDashTime + dashCooldown)
        {
            float dashDir = move != 0 ? move : (transform.localScale.x >= 0 ? 1 : -1);

            // Raycast to check for walls in dash direction
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * dashDir, dashCheckDistance, wallLayer);

            if (hit.collider == null)
            {
                // No wall detected — perform dash
                rb.linearVelocity = new Vector2(dashDir * dashForce, rb.linearVelocity.y);
                lastDashTime = Time.time;
            }
        }
    }
}
