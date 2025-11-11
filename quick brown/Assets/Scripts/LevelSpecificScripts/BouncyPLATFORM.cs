using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceForce = 15f;        // how strong the bounce is
    public Vector2 bounceDirection = Vector2.up; // direction of bounce (default: straight up)
    public bool resetVerticalVelocity = true;    // if true, cancels downward motion first
    public bool playSound = true;          // optional — for sound effect later

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Rigidbody2D rb = collision.collider.attachedRigidbody;
        if (rb == null) return;

        // Optionally reset current velocity so bounce feels strong
        if (resetVerticalVelocity)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        // Apply bounce
        rb.AddForce(bounceDirection.normalized * bounceForce, ForceMode2D.Impulse);

       
    }

    // Visualize bounce direction in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(bounceDirection.normalized * 1f));
    }
}
