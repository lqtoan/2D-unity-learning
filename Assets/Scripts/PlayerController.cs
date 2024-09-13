using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float doubleTapTime = 0.3f;
    public float dashBoost = 100f;

    [Header("Jump Settings")]
    public float jumpForce = 7f;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float checkRadius = 0.2f;
    public float gravityScaleOnFall = 1.5f;
    public float fallThreshold = -10f;

    [Header("Animation")]
    public Animator animator;

    public bool isFacingRight { get; private set; } = true;

    private Rigidbody2D rb;
    private float lastDashTime;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool wasGrounded = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleFall();
        UpdateAnimator();
        CheckForGroundedAudio();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float speed = moveSpeed;

        if (moveInput != 0)
        {
            if (Time.time - lastDashTime < doubleTapTime)
            {
                speed *= dashBoost;
            }
            lastDashTime = Time.time;

            if (moveInput > 0 && !isFacingRight || moveInput < 0 && isFacingRight)
            {
                Flip();
            }
        }

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (rb.velocity.magnitude > 0.01f && isGrounded)
        {
            PlayRunningSound();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || canDoubleJump)
            {
                Jump();
                if (!isGrounded) canDoubleJump = false;
            }
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset vertical velocity for consistent jump height
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetTrigger("Jump");
        AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpClip);
    }

    private void HandleFall()
    {
        rb.gravityScale = rb.velocity.y < 0 ? gravityScaleOnFall : 1f;

        if (rb.position.y < fallThreshold)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
    }

    private void CheckForGroundedAudio()
    {
        if (!wasGrounded && isGrounded)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpClip);
        }
        wasGrounded = isGrounded;
    }

    private void PlayRunningSound()
    {
        if (!AudioManager.Instance.vfxAudioSrc.isPlaying)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.runClip);
        }
    }
}
