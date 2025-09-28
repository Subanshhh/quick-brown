using UnityEngine;

public class BackgroundController : MonoBehaviour
{

    private float startpos, length;
    [SerializeField] private GameObject cam;
    [SerializeField] private float ParallaxEffect;


    private void Start()
    {
        startpos = transform.position.x;
        length=GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float distance = cam.transform.position.x * ParallaxEffect;
        float movement = cam.transform.position.x * (1 - ParallaxEffect);

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        if (movement > startpos - length)
        {
            startpos += length;
        }
        else if (movement < startpos - length)
        {
            startpos -= length;
        }
    }





}
