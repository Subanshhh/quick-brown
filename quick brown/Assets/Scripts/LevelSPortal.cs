using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePortal : MonoBehaviour
{
    [Tooltip("Gravity direction the player should have after entering this portal")]
    public Vector2 gravityDirection = Vector2.down;

    private bool playerNearby = false;
    private WallWalkingCharacterController player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            player = other.GetComponent<WallWalkingCharacterController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            player = null;
        }
    }

    private void Update()
    {
        if (playerNearby && player != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            player.SetGravity(gravityDirection);
        }
    }
}
