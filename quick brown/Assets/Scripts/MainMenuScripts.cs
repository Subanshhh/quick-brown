
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine;

public class MainMenuScripts : MonoBehaviour
{
    private char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ*".ToCharArray();

    [SerializeField] private Text LettersCollected;
    [SerializeField] private int TotalLevels;

    private void Awake()
    {
        for (int i = 0; i < 27; i++)
        {
            //if it has a key,
            if (i == 0 || PlayerPrefs.HasKey("Level" + alphabet[i - 1].ToString() + "Stars"))
            {
                PlayerPrefs.SetInt("LettersCollected", i);
            }
        }
    }


    private void Update()
    {
        int letterscollected = (PlayerPrefs.GetInt("LettersCollected", 0));
        if (letterscollected < 0)
        {
            letterscollected = 0;
        }
        LettersCollected.text = (letterscollected.ToString()) + "/" + TotalLevels.ToString();
    }



}
