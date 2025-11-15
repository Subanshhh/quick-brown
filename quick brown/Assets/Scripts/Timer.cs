using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [Header("Start condition")]
    [SerializeField] private bool startOnFirstPlayerInput = true;
    [SerializeField] private bool startOnFirstPhysicalMovement = false;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float velocityThreshold = 0.05f;

    [Header("Level-specific objects")]
    [SerializeField] private bool isLevelU = false; // set in inspector
    [SerializeField] private GameObject objectToDestroyAt45; // object to remove
    [SerializeField] private GameObject objectToAppearAt45; // object to spawn

    [Header("Eggs (appear at 15s, 25s, 35s)")]
    [SerializeField] private GameObject egg15;
    [SerializeField] private GameObject egg25;
    [SerializeField] private GameObject egg35;

    [HideInInspector] public float currentTime = 0f;
    private bool isRunning = false;
    private bool hasStarted = false;

    // triggers
    private bool triggered15 = false;
    private bool triggered25 = false;
    private bool triggered35 = false;
    private bool triggered45 = false;

    void Update()
    {
        // Start timer if needed
        if (!hasStarted)
        {
            if (startOnFirstPlayerInput && HasPlayerInput())
                StartTimer();
            else if (startOnFirstPhysicalMovement && playerRb != null && HasPlayerMoved())
                StartTimer();
        }

        if (!isRunning) return;

        currentTime += Time.deltaTime;
        UpdateTimerDisplay();

        if (isLevelU)
        {
            // Eggs at 15, 25, 35
            if (!triggered15 && currentTime >= 15f) { ActivateEgg(egg15); triggered15 = true; }
            if (!triggered25 && currentTime >= 25f) { ActivateEgg(egg25); triggered25 = true; }
            if (!triggered35 && currentTime >= 35f) { ActivateEgg(egg35); triggered35 = true; }

            // Main 45-second event
            if (!triggered45 && currentTime >= 45f)
            {
                TriggerLevelUEvents();
                triggered45 = true;
            }
        }
    }

    bool HasPlayerInput()
    {
        var kb = Keyboard.current;
        var mouse = Mouse.current;
        if (kb == null) return false;

        return (kb.aKey.isPressed || kb.dKey.isPressed || kb.leftArrowKey.isPressed || kb.rightArrowKey.isPressed)
             || (kb.spaceKey.wasPressedThisFrame)
             || (mouse != null && mouse.rightButton.wasPressedThisFrame);
    }

    bool HasPlayerMoved()
    {
        Vector2 v = playerRb.linearVelocity;
        return Mathf.Abs(v.x) > velocityThreshold || Mathf.Abs(v.y) > velocityThreshold;
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentTime * 100f) % 100f);
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }

    void ActivateEgg(GameObject egg)
    {
        if (egg != null)
            egg.SetActive(true);
    }

    void TriggerLevelUEvents()
    {
        if (objectToDestroyAt45 != null)
            Destroy(objectToDestroyAt45);

        if (objectToAppearAt45 != null)
            objectToAppearAt45.SetActive(true);
    }

    // Controls
    public void StartTimer()
    {
        hasStarted = true;
        isRunning = true;
    }

    public void StopTimer() => isRunning = false;

    public void ResetTimer()
    {
        currentTime = 0f;
        hasStarted = false;
        isRunning = false;
        triggered15 = triggered25 = triggered35 = triggered45 = false;
        UpdateTimerDisplay();
    }

    public float GetTime() => currentTime;
}
