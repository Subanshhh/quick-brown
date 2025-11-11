using UnityEngine;
using System.Collections;

public class PredefinedFallingObjects : MonoBehaviour
{
    [System.Serializable]
    public class FallingEvent
    {
        public GameObject prefab;     // egg, mushroom, letter, etc.
        public float spawnTime;       // time in seconds when it should spawn
        public float xPosition;       // relative to platform center
        public float fallSpeed = 5f;  // speed of falling
    }

    public Transform platformCenter; // reference point for xPosition
    public FallingEvent[] fallingEvents; // array of events in order
    public float destroyY = -5f;    // Y position below which objects disappear

    [SerializeField] private GameObject backupEgg;
    private bool hasEgg = false;

    int x = 0;


    void Start()
    {
        hasEgg = false;
        foreach (var ev in fallingEvents)
        {
            StartCoroutine(SpawnEvent(ev));
        }
    }

    IEnumerator SpawnEvent(FallingEvent ev)
    {

        yield return new WaitForSeconds(ev.spawnTime);

        // Instantiate object
        GameObject obj = Instantiate(ev.prefab);
        obj.transform.position = new Vector3(platformCenter.position.x + ev.xPosition, platformCenter.position.y + 5f, 0); // spawn above platform
        print("Spawned " + obj.name + x);
        x++;
        // Add Rigidbody2D if not already
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }

        // Start falling
        rb.linearVelocity = Vector2.down * ev.fallSpeed;

        // Destroy object after it falls below destroyY
        StartCoroutine(DestroyWhenBelow(obj, destroyY));
    }
    IEnumerator DestroyWhenBelow(GameObject obj, float yLevel)
    {
        while (obj != null && obj.transform.position.y > yLevel)
        {
            yield return null;
        }

        if (obj != null)
        {
            // Check if any child has tag "Egg"
            
            foreach (Transform child in obj.transform)
            {
                if (child.CompareTag("Egg"))
                {
                    hasEgg = true;
                    break;
                }
            }

            // If egg exists, activate backupEgg
            if (hasEgg && backupEgg != null)
            {
                backupEgg.SetActive(true);
            }

            // Destroy the prefab in all cases
            Destroy(obj);
        }
    }

}
