using UnityEditor.PackageManager.UI;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine;

public class MainMenuScripts : MonoBehaviour
{

    [SerializeField] private Text LettersCollected;
    [SerializeField] private int TotalLevels;

    private void Update()
    {
        int letterscollected = (PlayerPrefs.GetInt("LettersCollected", 0) - 1);
        if (letterscollected < 0)
        {
            letterscollected = 0;
        }
        LettersCollected.text = (letterscollected.ToString()) + "/" + TotalLevels.ToString();
    }



}
