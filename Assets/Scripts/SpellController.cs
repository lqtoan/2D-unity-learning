using System.Collections;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float lifeTime = 5f;

    // public GameObject impactEffect; 

    private Rigidbody2D rb;
    private PlayerController playerController;

    private void Awake()
    {
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(this.playerController.isFacingRight) {
            this.rb.velocity = transform.right * this.speed;
        } else {
            this.rb.velocity = -1 * transform.right * this.speed;
        }
        StartCoroutine(DestroyAfterLifetime());
    }

    // void OnTriggerEnter2D(Collider2D hitInfo)
    // {
    // Kiểm tra xem phép thuật chạm vào cái gì
    // Enemy enemy = hitInfo.GetComponent<Enemy>(); // Giả sử có đối tượng "Enemy"
    // if (enemy != null)
    // {
    //     enemy.TakeDamage(damage); // Gây sát thương cho kẻ thù
    // }

    // if (impactEffect != null)
    // {
    //     Instantiate(impactEffect, transform.position, transform.rotation); // Tạo hiệu ứng khi phép thuật va chạm
    // }

    // Destroy(gameObject); // Hủy phép thuật sau khi va chạm
    // }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        if (this != null)
        {
            Destroy(gameObject);
        }
    }
}
