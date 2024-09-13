using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float speed = 100f;
    private float lifeTime = 5f;
    private Rigidbody2D rb;
    private PlayerController playerController;

    private void Awake()
    {
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        this.Fire();
        StartCoroutine(DestroyAfterLifetime());
    }

    private void Fire()
    {
        if (this.playerController.isFacingRight) {
            this.rb.velocity = transform.right * this.speed;
        } else {
            this.rb.velocity = -transform.right * this.speed;
        }
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        if (this != null)
        {
            Destroy(gameObject);
        }
    }
}
