using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collected! +" + value);
            Destroy(gameObject);
        }
    }
}
