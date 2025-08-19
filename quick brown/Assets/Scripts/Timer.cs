using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // for Keyboard/Mouse (Input System)

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;   // assign your TMP text
    [Header("Start condition")]
    [SerializeField] private bool startOnFirstPlayerInput = true;

    // Optional: start when the player actually moves (assign playerRb & toggle below)
    [SerializeField] private bool startOnFirstPhysicalMovement = false;
    [SerializeField] private Rigidbody2D playerRb; // optional
    [SerializeField] private float velocityThreshold = 0.05f;

    private float currentTime = 0f;
    private bool isRunning = false;   // don't run until we detect movement
    private bool hasStarted = false;  // ensures we only start once

    void Update()
    {
        // Arm the timer when the player first acts
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
    }

    bool HasPlayerInput()
    {
        var kb = Keyboard.current;
        var mouse = Mouse.current;
        if (kb == null) return false;

        return (kb.aKey.isPressed || kb.dKey.isPressed || kb.leftArrowKey.isPressed || kb.rightArrowKey.isPressed)
             || (kb.spaceKey.wasPressedThisFrame) // jump
             || (mouse != null && mouse.rightButton.wasPressedThisFrame); // dash
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

    // Controls (still available if you ever want to call them)
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
        UpdateTimerDisplay();
    }

    public float GetTime() => currentTime;
}
