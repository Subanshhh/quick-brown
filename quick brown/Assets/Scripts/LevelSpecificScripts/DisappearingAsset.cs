using UnityEngine;

public class TogglePlatformVisibleOnly : MonoBehaviour
{
    public float interval = 3f; // seconds between toggle
    private Renderer[] renderers;

    void Start()
    {
        // Get all renderers in this object and children
        renderers = GetComponentsInChildren<Renderer>();
        InvokeRepeating(nameof(Toggle), interval, interval);
    }

    void Toggle()
    {
        bool currentlyVisible = renderers.Length > 0 && renderers[0].enabled;

        // Toggle only the renderers
        foreach (Renderer r in renderers)
            r.enabled = !currentlyVisible;
    }
}
