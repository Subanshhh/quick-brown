using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class CrumbleTilemap : MonoBehaviour
{
    public float crumbleDelay = 2f;
    public float respawnDelay = 3f;

    private TilemapRenderer tilemapRenderer;
    private TilemapCollider2D tilemapCollider;
    private bool isCrumbling = false;

    void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCrumbling && collision.gameObject.CompareTag("Player"))
            StartCoroutine(CrumbleRoutine());
    }

    IEnumerator CrumbleRoutine()
    {
        isCrumbling = true;

        yield return new WaitForSeconds(crumbleDelay);

        tilemapRenderer.enabled = false;
        tilemapCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        tilemapRenderer.enabled = true;
        tilemapCollider.enabled = true;

        isCrumbling = false;
    }
}
