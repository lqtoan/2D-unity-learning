using UnityEngine;

public class EnemyController : MonoBehaviour 
{
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    [SerializeField] private Transform groundForwardCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float speed = 4f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();

        if (!IsGroundAhead()) 
        {
            FlipDirection();
        }

        if (IsWallAhead()) 
        {
            FlipDirection();
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * speed, rb.velocity.y);
    }

    private bool IsGroundAhead()
    {
        return Physics2D.Raycast(groundForwardCheck.position, Vector2.down, 1f, whatIsGround);
    }
    private bool IsWallAhead()
    {
        return Physics2D.Raycast(groundForwardCheck.position, isFacingRight ? Vector2.right : Vector2.left, 0.2f, whatIsGround);
    }

    private void FlipDirection()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(groundForwardCheck.position, groundForwardCheck.position + Vector3.down * 1f);
    //     Gizmos.DrawLine(groundForwardCheck.position, groundForwardCheck.position + (isFacingRight ? Vector3.right : Vector3.left) * 0.2f);
    // }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // DealDamageToPlayer(collision.gameObject);
            Destroy(collision.gameObject);
            Destroy(gameObject);
            FindObjectOfType<EnemySpawner>().DecreaseEnemyCount();
        }
    }

    // private void DealDamageToPlayer(GameObject player)
    // {
        // PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        // if (playerHealth != null)
        // {
        //     playerHealth.TakeDamage(10);
        // }
    // }
}
