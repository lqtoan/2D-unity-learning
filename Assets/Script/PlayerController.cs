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


    private void Awake()
    {
        // this.audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        this.animator.SetFloat("yVelocity", this.rb.velocity.y);
        this.animator.SetFloat("magnitude", this.rb.velocity.magnitude);

        if (!this.wasGrounded && this.isGrounded)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpClip);
        }
        this.wasGrounded = this.isGrounded;

        if (this.rb.velocity.magnitude > 0.1f && this.isGrounded)
        {
            if (!AudioManager.Instance.vfxAudioSrc.isPlaying)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.runClip);
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
        this.rb.velocity = new Vector2(moveInput * speed, this.rb.velocity.y);

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
        this.isGrounded = Physics2D.OverlapCircle(this.groundCheck.position, this.checkRadius, whatIsGround);
        if (this.isGrounded)
        {
            this.canDoubleJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (this.isGrounded)
            {
                Jump();
                AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpClip);
            }
            else if (this.canDoubleJump)
            {
                Jump();
                this.canDoubleJump = false;
            }
        }
    }

    private void Jump()
    {
        this.animator.SetTrigger("Jump");
        this.rb.velocity = new Vector2(this.rb.velocity.x, 0f); // Reset vertical velocity to ensure consistent jump height
        this.rb.AddForce(Vector2.up * this.jumpForce, ForceMode2D.Impulse);
    }
}
