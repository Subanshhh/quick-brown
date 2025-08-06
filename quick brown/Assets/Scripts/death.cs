using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnTouch : MonoBehaviour
{
   
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
