using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCollisionHandling player = collision.GetComponent<PlayerCollisionHandling>();
            if (player != null)
            {
                player.TakeDamage(damage);

                // TODO: obj pooling
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        // TODO: obj pooling
        // Destroy(gameObject, 2f);
    }
}
