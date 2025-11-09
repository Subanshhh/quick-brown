using UnityEngine;

public class TilemapRotater : MonoBehaviour
{
    public Transform player;  // the pivot point

    public void RotateLevel(int rotationAngle)
    {
        // Rotate around the player's position on Z-axis
        transform.RotateAround(player.position, Vector3.forward, rotationAngle);
    }
}
