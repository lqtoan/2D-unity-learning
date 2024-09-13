using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float speed = 50f;
    private float lifeTime = 5f;
    private Rigidbody2D rb;
    private PlayerController playerController;
    private bool isFacingRight;

    private void Awake()
    {
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    private void Update()
    {
    }

    private void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.isFacingRight = playerController.isFacingRight;

        this.Fire();
        StartCoroutine(DestroyAfterLifetime());
    }

    private void Fire()
    {
        this.rb.velocity = (this.isFacingRight ? 1 : -1) * transform.right * this.speed;
        transform.localScale = new Vector3((this.isFacingRight ? 1 : -1) * transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        if (this != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
        {
            Debug.Log("cham");
            Destroy(gameObject); 
        }
    }
}
