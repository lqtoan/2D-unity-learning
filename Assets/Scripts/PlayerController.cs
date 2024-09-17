using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Animation
    [Header("Animation")]
    public Animator animator;
    #endregion

    public bool isFacingRight { get; private set; } = true;
    [SerializeField] private Slider staminaSlider; // Reference to the UI slider

    #region Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashBoost = 2f;
    #endregion

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 20f;
    [SerializeField] private float staminaRegenRate = 2f;  // Stamina regenerated per second
    [SerializeField] private float staminaConsumptionRate = 10f; // Stamina consumed per second while boosting
    [SerializeField] private float minStaminaForBoost = 0.1f; // Minimum stamina required to start boosting
    private TrailRenderer trailRenderer;

    #region Jump Settings
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private float gravityScaleOnFall = 1.5f;
    [SerializeField] private float fallThreshold = -5f;
    #endregion

    #region Double Tap Settings
    [Header("Double Tap Settings")]
    [SerializeField] private float doubleTapThreshold = 0.3f; // Time in seconds to consider a double tap
    private float lastTapTime = 0f;
    private bool isBoosting = false;
    #endregion

    private Rigidbody2D rb;
    private float currentStamina;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool wasGrounded = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina; // Initialize stamina
        trailRenderer = GetComponent<TrailRenderer>();

        if (staminaSlider == null)
        {
            // Tìm Slider từ Canvas
            staminaSlider = GameObject.Find("Stamia").GetComponent<Slider>();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleFall();
        UpdateAnimator();
        CheckForGroundedAudio();
        RegenerateStamina(); // Regenerate stamina when not boosting
        UpdateStaminaUI();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float speed = moveSpeed;

        if (moveInput != 0)
        {
            // Check for double-tap
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - lastTapTime <= doubleTapThreshold)
                {
                    isBoosting = true;
                    if (trailRenderer != null) trailRenderer.emitting = true;
                }
                else
                {
                    isBoosting = false;
                    if (trailRenderer != null) trailRenderer.emitting = false;
                }
                lastTapTime = Time.time;
            }

            // Apply boost if double-tap detected
            if (isBoosting && currentStamina > minStaminaForBoost)
            {
                speed *= dashBoost; // Increase speed to run
                UseStamina();
            }
            else
            {
                isBoosting = false; // Reset boosting state if no longer applicable
            }

            // Flip the character based on direction
            if (moveInput > 0 && !isFacingRight || moveInput < 0 && isFacingRight)
            {
                Flip();
            }
        }

        // Apply velocity based on movement input and speed
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Play running sound if moving and grounded
        if (rb.velocity.magnitude > 0.01f && isGrounded)
        {
            PlayRunningSound();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    private void UseStamina()
    {
        currentStamina -= staminaConsumptionRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Clamp stamina between 0 and max
    }

    private void RegenerateStamina()
    {
        // Regenerate stamina
        if (!isBoosting)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Clamp stamina between 0 and max
        }
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

    private void UpdateStaminaUI()
    {
        staminaSlider.value = currentStamina / maxStamina; // Update the slider value based on stamina
    }
}
