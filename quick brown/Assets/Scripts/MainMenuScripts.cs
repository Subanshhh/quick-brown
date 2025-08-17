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
        LettersCollected.text = (PlayerPrefs.GetInt("LettersCollected").ToString() ) +"/" + TotalLevels.ToString();
    }



}
