using UnityEngine;
using TMPro;

public class HideTextOnMove : MonoBehaviour
{
    public Transform player;     // Player's transform
    public TMP_Text uiText;      // The TMP text you want to hide
    public float moveThreshold = 0.1f; // How much movement counts as "moving"

    private Vector3 lastPosition;
    private bool hasMoved = false;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set!");
            enabled = false;
            return;
        }

        if (uiText == null)
        {
            Debug.LogError("UI Text reference not set!");
            enabled = false;
            return;
        }

        lastPosition = player.position;
    }

    void Update()
    {
        if (!hasMoved)
        {
            // Check if player has moved more than threshold
            if (Vector3.Distance(player.position, lastPosition) > moveThreshold)
            {
                hasMoved = true;
                uiText.gameObject.SetActive(false);
            }
        }
    }
}
