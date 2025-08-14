using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    [SerializeField] private GameObject Star1;
    [SerializeField] private GameObject Star2;
    [SerializeField] private GameObject Star3;


    [SerializeField] private Sprite StarCompletedSprite;
    [SerializeField] private Sprite StarFailedSprite;


    private void Update()
    {
        int x = NumberOfStarsPerLevel();

        Star1.GetComponent<Image>().sprite = StarFailedSprite;
        Star2.GetComponent<Image>().sprite = StarFailedSprite;
        Star3.GetComponent<Image>().sprite = StarFailedSprite;


        if (x >= 1) Star1.GetComponent<Image>().sprite = StarCompletedSprite;
        if (x >= 2) Star2.GetComponent<Image>().sprite = StarCompletedSprite;
        if (x == 3) Star3.GetComponent<Image>().sprite = StarCompletedSprite;

    }

    private int NumberOfStarsPerLevel()
    {
        string levelName = GetComponentInChildren<Text>().text;
        levelName = "Level" + levelName + "Stars";

        return PlayerPrefs.GetInt(levelName , 0);

    }



}
