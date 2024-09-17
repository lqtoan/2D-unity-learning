using System.Collections;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float damage = 1f;
    private ObjectPool objectPool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCollisionHandling player = collision.GetComponent<PlayerCollisionHandling>();
            if (player != null)
            {
                player.TakeDamage(damage);

                // objectPool.ReturnObject(gameObject);
                gameObject.SetActive(false);
            } 
            else
            {
                Debug.LogError("PlayerCollisionHandling component not found on the object with tag ");
            }
        }
    }

    private void Start()
    {
        DestroyEnemyBullet();
    }

    private IEnumerator DestroyEnemyBullet()
    {
        yield return new WaitForSeconds(2f);
        objectPool.ReturnObject(gameObject);
    }
}
