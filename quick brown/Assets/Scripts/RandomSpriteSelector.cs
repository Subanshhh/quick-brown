using UnityEngine;

public class RandomSpriteSelector : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0,sprites.Length)];
    }
}
