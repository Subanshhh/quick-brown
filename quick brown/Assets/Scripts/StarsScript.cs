using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StarsScript : MonoBehaviour
{
    public GameObject Panel;
    [SerializeField] private Image Star0;
    [SerializeField] private Image Star1;
    [SerializeField] private Image Star2;



    private void Awake()
    {
        Panel.SetActive(false);
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





        if (true) // replace with your actual condition for star 1
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






        if (true) // replace with your actual condition for star 2
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
