using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    [Header("Dead Zone")]
    public Vector2 deadZoneSize = new Vector2(2f, 1f); // width, height of the deadzone box

    [Header("Smoothing")]
    public float smoothTime = 0.15f; // lower = snappier, higher = smoother
    private Vector3 velocity = Vector3.zero;

    [Header("Look Ahead")]
    public float lookAheadDistance = 2f;  // how far ahead camera shifts when moving
    public float lookAheadSpeed = 3f;

    private Vector3 currentLookAhead;
    private Vector3 targetPos;

    void LateUpdate()
    {
        if (!player) return;

        Vector3 camPos = transform.position;

        // --- Dead Zone ---
        Vector3 diff = player.position - camPos;
        if (Mathf.Abs(diff.x) > deadZoneSize.x) camPos.x = player.position.x;
        if (Mathf.Abs(diff.y) > deadZoneSize.y) camPos.y = player.position.y;

        // --- Look Ahead (anticipation) ---
        float moveDir = Input.GetAxisRaw("Horizontal");
        Vector3 targetLookAhead = new Vector3(moveDir * lookAheadDistance, 0, 0);
        currentLookAhead = Vector3.Lerp(currentLookAhead, targetLookAhead, Time.deltaTime * lookAheadSpeed);

        // --- Final Target ---
        targetPos = new Vector3(camPos.x, camPos.y, transform.position.z) + currentLookAhead;

        // --- Smooth Damp ---
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    private void OnDrawGizmosSelected()
    {
        // visualize dead zone box
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, deadZoneSize * 2);
    }
}
