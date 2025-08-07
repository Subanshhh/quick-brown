using UnityEngine;
using UnityEngine.InputSystem;

public class LetterCollection : MonoBehaviour
{

    public Key requiredKey; 

    private bool playerNearby = false;

    void Update()
    {
        if (!playerNearby) return;

        if (Keyboard.current[requiredKey].wasPressedThisFrame)
        {
            CollectLetter();
            GameObject.Find("CanvasUI").GetComponentInChildren<StarsScript>().FinishedLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log($"Player near letter '{requiredKey}'. Press '{requiredKey}' to collect!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    private void CollectLetter()
    {
        Debug.Log($"Letter '{requiredKey}' collected!");
        Destroy(gameObject);
    }
}
