using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public GameObject dialogueBox;
    public TMP_Text dialogueText;

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialogueBox.SetActive(false);
    }

    public void ShowNextLine(string[] lines, ref int index)
    {
        dialogueBox.SetActive(true);

        // If still typing → finish instantly on F press
        if (isTyping)
        {
            CompleteTyping();
            return;
        }

        // If finished last line → close window
        if (index >= lines.Length)
        {
            HideDialogue();
            return;
        }

        // Start typing next line
        StartTyping(lines[index]);
        index++;
    }

    private void StartTyping(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypewriterEffect(line));
    }

    private IEnumerator TypewriterEffect(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        // Finish instantly
        StopCoroutine(typingCoroutine);
        typingCoroutine = null;

        // Reveal full line
        dialogueText.maxVisibleCharacters = int.MaxValue;
        dialogueText.text = dialogueText.text = dialogueText.text; // just forces refresh

        isTyping = false;
    }

    public void HideDialogue()
    {
        dialogueBox.SetActive(false);
        dialogueText.text = "";
    }
}
