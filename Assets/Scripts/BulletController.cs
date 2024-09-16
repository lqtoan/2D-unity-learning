using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 50f;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody2D rb;
    private bool isFacingRight;
    private ObjectPool objectPool;

    private void Awake()
    {
        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        isFacingRight = playerController.isFacingRight;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Fire();
        StartCoroutine(DestroyAfterLifetime());
        objectPool = FindObjectOfType<ObjectPool>();
    }

    private void Fire()
    {
        float direction = isFacingRight ? 1f : -1f;
        rb.velocity = direction * transform.right * speed;
        // transform.localScale = new Vector2(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        // Destroy(gameObject);
        objectPool.ReturnObject(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Ground") || collider.CompareTag("Ground"))
        {
            // Destroy(gameObject); 
            objectPool.ReturnObject(gameObject);
        }
    }
}
