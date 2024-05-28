using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField]private float speed ;
    [SerializeField] private float jumpingPower;
    private bool isFacingRight = true;
    private bool jump = true;
    [SerializeField] private float maxVel;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    private bool isJumping;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpForce;
    private float Fsp;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(0f, 0f);

    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    // Dash variables
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashCooldown = 2f;
    private bool isDashing;
    private float lastDashTime;

    // Double jump variables
    private bool canDoubleJump;
    [SerializeField] private float doubleJumpPower = 15f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (rb.velocity.magnitude < maxVel)
            rb.AddForce(new Vector2(horizontal, 0f) * speed, ForceMode2D.Force);

        HandleJump();
        HandleDash();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Implement movement logic if necessary
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (jump)
            {
                rb.AddForce(new Vector2(0, 1) * jumpingPower + wallJumpingPower, ForceMode2D.Impulse);
                jump = false;
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity before double jump
                rb.AddForce(new Vector2(0, 1) * doubleJumpPower, ForceMode2D.Impulse);
                canDoubleJump = false;
            }
        }
    }

    private void HandleDash()
    {
        if (Input.GetButtonDown("Dash") && !isDashing && Time.time > lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(horizontal * dashForce, 0f);

        yield return new WaitForSeconds(0.2f);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Mathf.Log(groundLayer, 2))
        {
            jump = true;
            canDoubleJump = true;
        }
        if (collision.gameObject.layer == Mathf.Log(wallLayer, 2))
        {
            if (rb.velocity.x < 0f)
            {
                wallJumpingPower.x = 10f;
            }
            else
            {
                wallJumpingPower.x = -10f;
            }

            jump = true;
        }
    }
}
