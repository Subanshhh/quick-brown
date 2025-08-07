using UnityEngine;
using UnityEngine.UI;


public class StarsScript : MonoBehaviour
{
    public GameObject Panel { get; private set; }
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
        if (true) //if you get the star (add condition for stars) //for star 0 
        {
            Color imageColor = Star0.color;
            imageColor.a = 0.2f;
        }
        else
        {
            Color imageColor = Star0.color;
            imageColor.a = 0.2f;
        }


        if (true) //if you get the star (add condition for stars) //for star 1
        {
            Color imageColor = Star1.color;
            imageColor.a = 0.2f;
        }
        else
        {
            Color imageColor = Star1.color;
            imageColor.a = 0.2f;
        }
        if (true) //if you get the star (add condition for stars) //for star 2
        {
            Color imageColor = Star2.color;
            imageColor.a = 0.2f;
        }
        else
        {
            Color imageColor = Star2.color;
            imageColor.a = 0.2f;
        }


    }







}
