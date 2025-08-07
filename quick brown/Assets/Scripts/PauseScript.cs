using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScript : MonoBehaviour
{

    [SerializeField] private GameObject PausePanel;

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && GetComponent<StarsScript>().Panel.activeInHierarchy == false)
        {
            PausePanel.SetActive(!PausePanel.activeInHierarchy); 
        }
        if (PausePanel.activeInHierarchy || GetComponent<StarsScript>().Panel.activeInHierarchy)
            Time.timeScale = 0;
        else 
            Time.timeScale = 1;
        
    }
    public void TogglePause()
    {
        PausePanel.SetActive(!PausePanel.activeInHierarchy);
    }

}
