using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class Death : MonoBehaviour
{
    [SerializeField] private string DeathTag;
    private ParticleSystem deathParticles;

    private void Awake()
    {
        deathParticles = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(DeathTag)) OnDeath();
    }

    public void OnDeath()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log("Starting death func");
        StartCoroutine(DeathSequence());
    }
    public IEnumerator DeathSequence()
    {
        Debug.Log("Death animation playing");

        deathParticles.Play();
        yield return new WaitWhile(() => deathParticles.isPlaying);

        Debug.Log("Death animation finished");

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
