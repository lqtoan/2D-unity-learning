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
    public Slider staminaSlider; // Reference to the UI slider

    #region Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashBoost = 4f;
    #endregion

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 20f;
    [SerializeField] private float staminaRegenRate = 1f;  // Stamina regenerated per second
    [SerializeField] private float staminaConsumptionRate = 5f; // Stamina consumed per second while boosting
    [SerializeField] private float minStaminaForBoost = 0.1f; // Minimum stamina required to start boosting

    #region Jump Settings
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private float gravityScaleOnFall = 1.5f;
    [SerializeField] private float fallThreshold = -5f;
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

        Debug.Log(currentStamina);
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float speed = moveSpeed;

        if (moveInput != 0)
        {
            // Check if the player is holding down Left Shift to run
            if (Input.GetKey(KeyCode.LeftShift) && currentStamina > minStaminaForBoost)
            {
                speed *= dashBoost; // Increase speed to run
                UseStamina();
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
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void UseStamina()
    {
        currentStamina -= staminaConsumptionRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Clamp stamina between 0 and max
    }

    private void RegenerateStamina()
    {
        // Regenerate stamina only if not boosting
        if (!Input.GetKey(KeyCode.LeftShift) || rb.velocity.magnitude == 0)
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
