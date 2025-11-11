using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing = false;
    private float dashEndTime = 0f;
    private float lastDashTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get movement input
        moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y = -1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x = 1f;
        moveInput = moveInput.normalized;

        // Dash input
        if ((Mouse.current.rightButton.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
            && Time.time >= lastDashTime + dashCooldown && !isDashing && moveInput != Vector2.zero)
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            rb.linearVelocity = moveInput * dashSpeed;
            lastDashTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            if (Time.time >= dashEndTime)
            {
                isDashing = false;
            }
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }
}
