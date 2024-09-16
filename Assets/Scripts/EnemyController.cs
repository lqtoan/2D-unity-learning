using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected float speed = 4f;
    [SerializeField] protected float jumpForce = 10f;
    [SerializeField] protected float maxHealth = 2;
    [SerializeField] protected Animator animator;
    [SerializeField] protected EnemyHealthBar healthBar;
    [SerializeField] private Transform groundForwardCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private ObjectPooler objectPooler; // Thêm tham chiếu đến ObjectPooler
    protected float currentHealth;
    protected Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isFacingRight = true;
    private Color originalColor;
    private readonly Color damageColor = Color.red;
    private readonly float flashDuration = 0.2f;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on the enemy object!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the enemy object!");
        }

        healthBar = GetComponent<EnemyHealthBar>();
        if (healthBar == null)
        {
            Debug.LogError("EnemyHealthBar not found! Make sure it's attached to the enemy.");
        }

        originalColor = spriteRenderer.color;
        currentHealth = maxHealth;
        healthBar?.SetHealth(currentHealth, maxHealth);
    }

    protected void Update()
    {
        Move();

        if (!IsGroundAhead(groundForwardCheck, whatIsGround))
        {
            FlipDirection();
        }

        if (IsWallAhead(groundForwardCheck, isFacingRight, whatIsGround))
        {
            FlipDirection();
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * speed, rb.velocity.y);
    }

    protected bool IsGroundAhead(Transform checkPosition, LayerMask groundLayer)
    {
        return Physics2D.Raycast(checkPosition.position, Vector2.down, 1f, groundLayer);
    }

    private bool IsWallAhead(Transform checkPosition, bool facingRight, LayerMask groundLayer)
    {
        return Physics2D.Raycast(checkPosition.position, facingRight ? Vector2.right : Vector2.left, 0.2f, groundLayer);
    }

    private void FlipDirection()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            float damage = 1;  // Giả sử viên đạn gây 1 đơn vị sát thương
            currentHealth = CalculateHealth(currentHealth, damage);  // Tính máu mới

            healthBar.SetHealth(currentHealth, maxHealth);  // Cập nhật thanh máu
            StartCoroutine(FlashDamageEffect());  // Gọi hiệu ứng chớp khi trúng đòn
            animator.SetTrigger("TakeDamage");  // Kích hoạt animation trúng đòn
            Destroy(collision.gameObject);  // Hủy viên đạn

            if (currentHealth <= 0)
            {
                // Trả đối tượng về pool thay vì hủy
                gameObject.SetActive(false);
                objectPooler.ReturnToPool(gameObject);

                FindObjectOfType<EnemySpawner>().DecreaseEnemyCount();
            }
        }
    }

    private float CalculateHealth(float currentHealth, float damage)
    {
        return Mathf.Max(currentHealth - damage, 0);  // Đảm bảo không âm
    }

    private IEnumerator FlashDamageEffect()
    {
        SetSpriteColor(damageColor);  // Thay đổi màu sang màu trúng đòn

        yield return new WaitForSeconds(flashDuration);

        SetSpriteColor(originalColor);  // Trả lại màu gốc
    }

    // Hàm thay đổi màu của SpriteRenderer
    private void SetSpriteColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
