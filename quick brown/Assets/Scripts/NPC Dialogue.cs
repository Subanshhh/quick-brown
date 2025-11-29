using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] dialogueLines;

    public GameObject fPrompt;   // assign your “F” prompt UI
    public AudioClip[] npcSFX;   // multiple random SFX for this NPC

    private int index = 0;
    private bool playerInRange = false;
    private bool dialogueStarted = false;

    public string npcName;   // Name that appears in the UI

    private void Start()
    {
        if (fPrompt != null)
            fPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Keyboard.current.fKey.wasPressedThisFrame)
        {
            // FIRST TIME PRESSING F — start dialogue
            if (!dialogueStarted)
            {
                dialogueStarted = true;
                DialogueManager.Instance.BeginDialogue();
                DialogueManager.Instance.SetName(npcName);
            }

            // Show next line FIRST
            bool ended = DialogueManager.Instance.ShowNextLine(dialogueLines, ref index);

            // If dialogue ended, reset and STOP — don't play SFX
            if (ended)
            {
                dialogueStarted = false;
                index = 0;
                return;    // 🚀 This prevents audio from playing
            }

            // Only here — play SFX for new line
            if (npcSFX != null && npcSFX.Length > 0)
            {
                int rand = Random.Range(0, npcSFX.Length);
                AudioManager.PlaySFXStatic(npcSFX[rand]);
            }
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

            dialogueStarted = false;
            index = 0;
        }
    }
}
