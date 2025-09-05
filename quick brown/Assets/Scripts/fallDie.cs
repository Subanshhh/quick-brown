using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnFall : MonoBehaviour
{
    private Transform player;
    public float lowerThreshold = -4f;
    public float upperThreshold = 10f;

    private void Awake()
    {
        player = GetComponent<Transform>();
    }

    void Update()
    {
        if (player != null && (player.position.y < lowerThreshold || player.position.y > upperThreshold))
        {
            RestartScene();
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
