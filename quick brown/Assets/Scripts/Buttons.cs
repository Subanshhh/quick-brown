using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }
    public void RemovePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    public void UnlockPlayerPrefs()
    {
        //all leters 
        char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ* ".ToCharArray();
        for (int i = 0; i < 27; i++)
        {
            PlayerPrefs.SetInt("Level" + alphabet[i] + "Stars", 3);
        }
    }


}
