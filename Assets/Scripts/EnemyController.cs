using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected float speed = 4f;
    [SerializeField] protected float maxHealth = 2;
    [SerializeField] protected Animator animator;
    [SerializeField] private Transform groundForwardCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private EnemyHealthBar healthBar;
    protected float currentHealth;
    protected bool isDead = false;
    protected ObjectPool objectPool;
    protected Rigidbody2D rb;
    protected bool isFacingRight = true;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private readonly Color damageColor = Color.red;
    private readonly float flashDuration = 0.2f;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        healthBar = GetComponent<EnemyHealthBar>();

        originalColor = spriteRenderer.color;
        currentHealth = maxHealth;
        healthBar?.SetHealth(currentHealth, maxHealth);

        objectPool = FindObjectOfType<ObjectPool>();
    }

    protected void Update()
    {
        if (isDead) return;

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

    protected virtual void Move()
    {
        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * speed, rb.velocity.y);
    }

    private bool IsGroundAhead(Transform checkPosition, LayerMask groundLayer)
    {
        return Physics2D.Raycast(checkPosition.position, Vector2.down, 1f, groundLayer);
    }

    private bool IsWallAhead(Transform checkPosition, bool facingRight, LayerMask groundLayer)
    {
        return Physics2D.Raycast(checkPosition.position, facingRight ? Vector2.right : Vector2.left, 0.2f, groundLayer);
    }

    protected void FlipDirection()
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
            collision.gameObject.SetActive(false);
            objectPool.ReturnObject(collision.gameObject);

            if (currentHealth == 0)
            {
                Die();
            }
        }
    }

    private float CalculateHealth(float currentHealth, float damage)
    {
        return Mathf.Max(currentHealth - damage, 0);
    }

    private IEnumerator FlashDamageEffect()
    {
        SetSpriteColor(damageColor);  
        yield return new WaitForSeconds(flashDuration);

        SetSpriteColor(originalColor);
    }

    private void SetSpriteColor(Color color)
    {
        spriteRenderer.color = color;
    }
    protected virtual void Die()
    {
        isDead = true;

        gameObject.SetActive(false);
        Reset();
        objectPool.ReturnObject(gameObject);

        FindObjectOfType<EnemySpawner>().DecreaseEnemyCount();
    }

    public void Reset()
    {
        isDead = false;
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth, maxHealth);
        healthBar.ResetHeartBar();
        spriteRenderer.color = originalColor;
        // isFacingRight = true;
        // transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        // rb.velocity = Vector2.zero;
    }
}
