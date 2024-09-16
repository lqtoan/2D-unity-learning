using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;

    private PlayerController playerController;
    private float lastFireTime;
    private ObjectPool objectPool;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found on the Player object. Please ensure the Player object is tagged correctly and has a PlayerController component.");
        }
    }

    private void Start()
    {
        objectPool = FindObjectOfType<ObjectPool>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanFire())
        {
            Fire();
        }
    }

    private bool CanFire()
    {
        return Time.time >= lastFireTime + fireRate;
    }

    private void Fire()
    {
        lastFireTime = Time.time;

        playerController?.animator.SetTrigger("RangedAttack");

        PlayFireSound();
        SpawnBullet();
    }

    private void PlayFireSound()
    {
        if (AudioManager.Instance != null && AudioManager.Instance.bowClip != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.bowClip);
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance or bowClip is missing. Please ensure they are properly assigned.");
        }
    }

    private void SpawnBullet()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            // Lấy đạn từ Object Pool
            GameObject bullet = objectPool.GetObject(bulletPrefab);

            if (bullet != null)
            {
                bool isFacingRight = playerController.isFacingRight; // Kiểm tra hướng của người chơi
                float direction = isFacingRight ? 1f : -1f;
                // Đặt vị trí và góc quay cho đạn
                bullet.transform.position = firePoint.position;
                bullet.transform.localScale = new Vector2(direction, 1f);

                // Nếu đạn có Rigidbody2D, khởi động lực bắn nếu cần
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                BulletController bulletController = bullet.GetComponent<BulletController>();

                if (bulletRb != null && bulletController != null)
                {
                    // Điều chỉnh hướng và tốc độ của viên đạn
                    bulletRb.velocity = direction * firePoint.right * bulletController.speed;
                }
                else
                {
                    Debug.LogError("BulletController or Rigidbody2D is missing on the bullet prefab.");
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve bullet from object pool.");
            }
        }
        else
        {
            Debug.LogWarning("BulletPrefab or FirePoint is not assigned. Please assign them in the Inspector.");
        }
    }

}
