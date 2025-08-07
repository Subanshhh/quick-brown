using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public GameObject loadingPanel;      // Assign your loading screen UI panel here
    public float displayTime =3f;       // Time in seconds to keep the panel visible

    void Start()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);      // Show loading screen
            Invoke("HideLoadingScreen", displayTime);  // Hide it after delay
        }
    }

    void HideLoadingScreen()
    {
        loadingPanel.SetActive(false);   // Hide loading screen
    }
}
