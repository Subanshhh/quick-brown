using UnityEditor.PackageManager.UI;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine;

public class MainMenuScripts : MonoBehaviour
{

    [SerializeField] private Text LettersCollected;

    private void Update()
    {
        LettersCollected.text = (PlayerPrefs.GetInt("LetersCollected").ToString() ) +"/23";
    }



}
