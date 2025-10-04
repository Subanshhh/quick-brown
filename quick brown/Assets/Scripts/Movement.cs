﻿using UnityEngine;
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

    [Header("Better Jump")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 3f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

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

    // Internals
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
        CheckGrounded();

        // Coyote time
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Jump buffer
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
            AudioManager.PlayFoxMove();
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1f;
            playerTransform.localScale = new Vector3(-1f, playerTransform.localScale.y, 1f);
            AudioManager.PlayFoxMove();
        }
    }

    private void CheckGrounded()
    {
        Vector2 checkDir = isUpsideDown ? Vector2.up : Vector2.down;

        RaycastHit2D hit = Physics2D.CircleCast(
            groundCheck.position,
            groundCheckRadius,
            checkDir,
            0.05f,
            groundLayer
        );

        if (hit.collider != null)
        {
            float angle = Vector2.Angle(hit.normal, Vector2.up);

            if (!isUpsideDown)
            {
                // Allow ground if slope < ~80°, but not vertical walls
                if (angle < 80f) { isGrounded = true; return; }
            }
            else
            {
                float angleUpside = Vector2.Angle(hit.normal, Vector2.down);
                if (angleUpside < 80f) { isGrounded = true; return; }
            }
        }

        isGrounded = false;
    }

    private void HandleJump()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            float jumpDirection = isUpsideDown ? -1f : 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpDirection);

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
    }

    private void ApplyBetterJumpPhysics()
    {
        if (!isUpsideDown)
        {
            if (rb.linearVelocity.y < 0) // falling
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            else if (rb.linearVelocity.y > 0 &&
                     !(Keyboard.current.spaceKey.isPressed || Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed))
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else
        {
            if (rb.linearVelocity.y > 0) // falling upwards
                rb.linearVelocity += Vector2.up * -Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            else if (rb.linearVelocity.y < 0 &&
                     !(Keyboard.current.spaceKey.isPressed || Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed))
                rb.linearVelocity += Vector2.up * -Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && ((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            if (dashTrail) dashTrail.emitting = false;
            isDashing = false;
            rb.linearVelocity = Vector2.zero;
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
