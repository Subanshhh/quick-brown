using UnityEngine;

public class TurnOnOffOnTrigger : MonoBehaviour
{
    [Header("Objects to toggle")]
    public GameObject[] targetObjects;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject obj in targetObjects)
                if (obj != null) obj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject obj in targetObjects)
                if (obj != null) obj.SetActive(false);
        }
    }
}
