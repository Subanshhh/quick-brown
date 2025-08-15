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
            if  (i == 0 || PlayerPrefs.HasKey("Level" + alphabet[i - 1].ToString() + "Stars") && i < 7) {button.GetComponent<Button>().interactable = true; }
            else { button.GetComponent<Button>().interactable = false;
                Transform star1 = button.transform.Find("Image");
                Transform star2 = button.transform.Find("Image (1)");
                Transform star3 = button.transform.Find("Image (2)");

                if (star1 != null) star1.gameObject.SetActive(false);
                if (star2 != null) star2.gameObject.SetActive(false);
                if (star3 != null) star3.gameObject.SetActive(false); }
        




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
