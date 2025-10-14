using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class FlappyFoxController : MonoBehaviour
{
    [Header("Flap Settings")]
    public float flapForce = 7f;
    public float gravityAccel = 30f;      // stronger so fox falls after flap
    public float maxFallSpeed = -10f;
    public float maxRiseSpeed = 10f;      // limit upward

    [Header("Scroll Settings")]
    public float scrollSpeed = 3f;

    [Header("Boundaries")]
    public float topLimit = 5f;
    public float bottomLimit = -5f;

    private Rigidbody2D rb;
    private bool isAlive = true;
    private bool flapRequested = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (!isAlive) return;

        // check for flap input
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            flapRequested = true;
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            flapRequested = true;
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        // horizontal movement
        var vel = rb.linearVelocity;
        vel.x = scrollSpeed;

        // apply gravity
        vel += Vector2.down * gravityAccel * Time.fixedDeltaTime;

        // apply flap if requested
        if (flapRequested)
        {
            vel.y = flapForce;
            flapRequested = false; // reset
        }

        // clamp speeds
        vel.y = Mathf.Clamp(vel.y, maxFallSpeed, maxRiseSpeed);

        rb.linearVelocity = vel;

        CheckBounds();
    }

    private void CheckBounds()
    {
        // top limit
        if (transform.position.y > topLimit)
        {
            transform.position = new Vector3(transform.position.x, topLimit, transform.position.z);
            var vel = rb.linearVelocity;
            vel.y = 0f;
            rb.linearVelocity = vel;
        }

        // bottom death
        if (transform.position.y < bottomLimit)
            Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) return;

        if (collision.collider.CompareTag("Deadly"))
            Die();
    }

    private void Die()
    {
        if (!isAlive) return;
        isAlive = false;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GameStats.deathCount++;

        Debug.Log("Fox died! Show game over or restart here.");
    }
}
