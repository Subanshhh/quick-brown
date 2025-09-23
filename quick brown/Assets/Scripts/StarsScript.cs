using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StarsScript : MonoBehaviour
{
    public GameObject Panel;
    [SerializeField] private Image Star0;
    [SerializeField] private Image Star1;
    [SerializeField] private Image Star2;
    [SerializeField] private int TimeToFinish;


    [SerializeField] private Text TimeNumerator;
    [SerializeField] private Text TimeDenomenator;



    private void Awake()
    {
        Panel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && Panel.activeInHierarchy)
            GetComponent<Buttons>().NextLevel();
    }




    public void FinishedLevel()
    {
        Panel.SetActive(true);
        Time.timeScale = 0f;

        int starsEarned = 0;

        // Check conditions for each star
        if (true) // replace with your actual condition for star 0
        {
            Color imageColor = Star0.color;
            imageColor.a = 1f; // Fully visible for star 0
            Star0.color = imageColor;
            starsEarned++;
        }
        else
        {
            Color imageColor = Star0.color;
            imageColor.a = 0.2f; // Dim for star 0
            Star0.color = imageColor;
        }





        TimeNumerator.text = (GetComponentInParent<Timer>().currentTime).ToString();
        TimeDenomenator.text = (Mathf.RoundToInt(TimeToFinish)).ToString();
        if (GetComponentInParent<Timer>().currentTime <= TimeToFinish) // replace with your actual condition for star 1
        {
            Color imageColor = Star1.color;
            imageColor.a = 1f; // Fully visible for star 1
            Star1.color = imageColor;
            starsEarned++;
        }
        else
        {
            Color imageColor = Star1.color;
            imageColor.a = 0.2f; // Dim for star 1
            Star1.color = imageColor;
        }






        if (GameObject.FindWithTag("Egg") == null) // replace with your actual condition for star 2 //collected all eggs
        {
            Color imageColor = Star2.color;
            imageColor.a = 1f; // Fully visible for star 2
            Star2.color = imageColor;
            starsEarned++;
        }
        else
        {
            Color imageColor = Star2.color;
            imageColor.a = 0.2f; // Dim for star 2
            Star2.color = imageColor;
        }

        if (starsEarned > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "Stars", 0))
        {
            // If the stars earned are more than the saved ones, update the PlayerPrefs
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Stars", starsEarned);
        }


    }







}
