using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float doubleTapTime = 0.3f; // Time window for double-tap
    public float dashBoost = 100f; // Multiplier for speed boost
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public Animator animator;


    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded = false;
    private float checkRadius = 0.2f;
    private bool canDoubleJump;
    private float lastDashTime;
    // private float lastTapTimeD;
    private bool isFacingRight = true; // Track the direction the player is facing
    private float threshold = -5f;
    private AudioManager audioManager;


    private void Awake()
    {
        this.audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        this.animator.SetFloat("yVelocity", rb.velocity.y);
        this.animator.SetFloat("magnitude", rb.velocity.magnitude);

        if (!this.wasGrounded && this.isGrounded)
        {
            this.audioManager.PlaySFX(this.audioManager.jumpClip);
        }
        this.wasGrounded = isGrounded;

        if (this.rb.velocity.magnitude > 0.1f && this.isGrounded)
        {
            if (!this.audioManager.vfxAudioSrc.isPlaying)
            {
                this.audioManager.PlaySFX(this.audioManager.runClip);
            }
        }

        if (this.rb.position.y < this.threshold) SceneManager.LoadScene(0);
    }

    private void FixedUpdate()
    {

    }

    private void HandleMovement()
    {
        float speed = this.moveSpeed;
        float moveInput = 0f;

        // Handle movement and double-tap for A key
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f; // Move left

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - this.lastDashTime < this.doubleTapTime)
                {
                    speed *= this.dashBoost; // Speed boost
                }
                this.lastDashTime = Time.time;
            }

            // Flip the player to face left if necessary
            if (this.isFacingRight)
            {
                Flip();
            }
        }

        // Handle movement and double-tap for D key
        if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f; // Move right

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - this.lastDashTime < this.doubleTapTime)
                {
                    speed *= this.dashBoost; // Speed boost
                }
                this.lastDashTime = Time.time;
            }

            // Flip the player to face right if necessary
            if (!this.isFacingRight)
            {
                Flip();
            }
        }
        // Debug.Log(speed);

        // Increase speed if Left Shift is held
        // if (Input.GetKey(KeyCode.LeftShift))
        // {
        //     speed += dashBoost;
        // }

        // Apply movement
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

    }

    private void Flip()
    {
        // Switch the way the player is facing
        this.isFacingRight = !this.isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
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
            if (isGrounded)
            {
                Jump();
                this.audioManager.PlaySFX(this.audioManager.jumpClip);
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
        }
    }

    private void Jump()
    {
        this.animator.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset vertical velocity to ensure consistent jump height
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

    }
}
