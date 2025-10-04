using UnityEngine;
using UnityEngine.InputSystem;

public class PortalInvertGravity : MonoBehaviour
{
    [Tooltip("Key the player must press to activate the portal")]
    public Key activateKey = Key.F;

    private bool playerNearby = false;
    private PlayerMovement playerMovement;

    void Update()
    {
        if (!playerNearby || playerMovement == null)
            return;

        if (Keyboard.current[activateKey].wasPressedThisFrame)
        {
            AudioManager.PlayPortalEnter(); 
            InvertGravity();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            playerMovement = other.GetComponent<PlayerMovement>();
            Debug.Log("Player entered portal! Press " + activateKey + " to invert gravity.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            playerMovement = null;
        }
    }

    private void InvertGravity()
    {
        if (playerMovement != null)
        {
            Rigidbody2D playerRb = playerMovement.GetComponent<Rigidbody2D>();
            playerRb.gravityScale *= -1; // Invert gravity

            // Flip sprite visually
            Vector3 scale = playerRb.transform.localScale;
            scale.y *= -1;
            playerRb.transform.localScale = scale;

            // Toggle isUpsideDown bool
            playerMovement.isUpsideDown = !playerMovement.isUpsideDown;

            Debug.Log("Gravity inverted! isUpsideDown = " + playerMovement.isUpsideDown);
        }
    }
}
