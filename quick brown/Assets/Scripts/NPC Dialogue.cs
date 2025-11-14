using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] dialogueLines;

    public GameObject fPrompt;   // assign your “F” prompt UI
    private int index = 0;
    private bool playerInRange = false;

    private void Start()
    {
        if (fPrompt != null)
            fPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Keyboard.current.fKey.wasPressedThisFrame)
        {
            DialogueManager.Instance.ShowNextLine(dialogueLines, ref index);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            index = 0;

            if (fPrompt != null)
                fPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueManager.Instance.HideDialogue();

            if (fPrompt != null)
                fPrompt.SetActive(false);
        }
    }
}
