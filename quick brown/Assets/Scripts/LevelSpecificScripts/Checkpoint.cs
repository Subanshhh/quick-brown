using UnityEngine;

public class SimpleCheckpointRespawn : MonoBehaviour
{
    [Header("Bounds for auto-respawn")]
    public float lowerThreshold = -40f;
    public float upperThreshold = 50f;

    private Vector3 respawnPoint;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position; // Starting point = initial spawn
    }

    void Update()
    {
        // If player goes too high or low, respawn
        if (transform.position.y < lowerThreshold || transform.position.y > upperThreshold)
        {
            Respawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            respawnPoint = collision.transform.position;
            Debug.Log("Checkpoint set at: " + respawnPoint);
        }
        else if (collision.CompareTag("Deadly"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // Reset position and velocity instantly
        transform.position = respawnPoint;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
