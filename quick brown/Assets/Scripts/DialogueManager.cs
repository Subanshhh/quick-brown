using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    [Header("Typing Settings")]
    public float typeSpeed = 0.03f;

    private bool isTyping = false;

    // Support both movement scripts
    private PlayerMovement normalMove;
    private PlayerMovementWallJump wallMove;
    public TMP_Text nameText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Find whichever player movement exists in the scene
        normalMove = FindFirstObjectByType<PlayerMovement>();
        wallMove = FindFirstObjectByType<PlayerMovementWallJump>();

        dialoguePanel?.SetActive(false);
    }

    private void SetMovement(bool state)
    {
        if (normalMove != null) normalMove.canMove = state;
        if (wallMove != null) wallMove.canMove = state;
    }

    public void BeginDialogue()
    {
        SetMovement(false); // Freeze player
    }

    public void EndDialogue()
    {
        SetMovement(true); // Unfreeze player
        HideDialogue();
    }

    /// <summary>
    /// Show the next line of dialogue. Returns true if dialogue is finished.
    /// </summary>
    public bool ShowNextLine(string[] lines, ref int index)
    {
        if (dialoguePanel != null && !dialoguePanel.activeSelf)
            dialoguePanel.SetActive(true);

        // All lines finished
        if (index >= lines.Length)
        {
            EndDialogue();
            return true;
        }

        // Start typing line
        StopAllCoroutines();
        StartCoroutine(TypeLine(lines[index]));
        index++;

        return false;
    }

    /// <summary>
    /// Type a single line letter by letter without skipping or breaking lines.
    /// </summary>
    private IEnumerator TypeLine(string line)
    {
        isTyping = true;

        // Step 1: Put entire line first so TMP lays it out once
        dialogueText.text = line;

        // Step 2: Hide it by showing 0 characters
        dialogueText.maxVisibleCharacters = 0;

        int totalChars = line.Length;

        // Step 3: Reveal gradually
        for (int i = 0; i < totalChars; i++)
        {
            dialogueText.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }


    public void HideDialogue()
    {
        StopAllCoroutines();
        dialoguePanel?.SetActive(false);
        dialogueText.text = "";
        dialogueText.maxVisibleCharacters = 0;

    }

    /// <summary>
    /// Optional: Instantly finish typing current line if player presses key
    /// </summary>
    public void SkipTyping(string currentLine)
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentLine;
            isTyping = false;
        }
    }
    public void SetName(string n)
    {
        if (nameText != null)
            nameText.text = n;
    }

}
