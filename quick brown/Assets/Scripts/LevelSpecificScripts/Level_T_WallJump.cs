using UnityEngine;

public class Level_T_WallJump : MonoBehaviour
{
    [Header("Wall Detection")]
    public LayerMask wallLayer;
    public float wallCheckDistance = 0.5f;
    public Transform wallCheckPoint;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.2f;

    [Header("Movement")]
    public float slideSpeed = 1f;
    public float wallJumpForce = 10f;
    public Vector2 wallJumpDirection = new Vector2(1f, 1.2f);

    private Rigidbody2D rb;
    private bool isTouchingWall;
    private bool isGrounded;
    private bool isSliding;
    private bool facingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckSurroundings();
    }

    private void CheckSurroundings()
    {
        isTouchingWall = Physics2D.Raycast(wallCheckPoint.position, transform.right, wallCheckDistance, wallLayer);
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }
}
