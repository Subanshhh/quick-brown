using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;                        // Reference to the player's transform
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Z offset to keep camera behind the scene
    public float smoothSpeed = 5f;

    private float initialY; // Store the starting Y position of the camera

    void Start()
    {
        // Capture the initial Y position so it doesn't change
        initialY = transform.position.y;

        player = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x + offset.x, initialY, offset.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
