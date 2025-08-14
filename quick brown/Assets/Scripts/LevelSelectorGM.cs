using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorGM : MonoBehaviour
{
    [SerializeField] private GameObject ButtonPrefab;
    [SerializeField] private GameObject Panel;

    private char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ* ".ToCharArray();
    private void Awake()
    {
        for (int i = 0; i < 27; i++)
        {
            print(i + " = " + alphabet[i].ToString());   
            GameObject button = Instantiate(ButtonPrefab,Panel.transform);
            button.GetComponentInChildren<Text>().text = alphabet[i].ToString();
            
            //if it has a key,
            if (i == 0 || PlayerPrefs.HasKey("Level" + alphabet[i - 1].ToString() + "Stars")) button.GetComponent<Button>().interactable = true;
            else button.GetComponent<Button>().interactable = false;

            
            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked("Level" + button.GetComponentInChildren<Text>().text));
        }
    }

    public void OnButtonClicked(string sceneName)
    {
        if (sceneName == "Level*")
            SceneManager.LoadScene("LevelAsterisk");
        else
            SceneManager.LoadScene(sceneName);
    }
}
