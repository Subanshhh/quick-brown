using UnityEngine;
using UnityEngine.InputSystem;

public class Level_S_Portal : MonoBehaviour
{
    [Tooltip("New gravity direction for the player when entering this portal")]
    //public Vector2 gravityDirection = Vector2.down;

    private WallWalkingCharacterController player;
    [SerializeField] private int RotateDegree = 90;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<WallWalkingCharacterController>();
            if (player == null)
                Debug.LogError("Player has no WallWalkingCharacterController attached!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void Update()
    {
        if (player == null) return;
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            GameObject.Find("Grid").GetComponent<TilemapRotater>().RotateLevel(RotateDegree);
        }
    }
}
