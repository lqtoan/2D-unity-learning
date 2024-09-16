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
        // Tìm PlayerController bằng cách tag
        playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found on the Player object. Ensure the Player object is tagged correctly and has a PlayerController component.");
        }
    }

    private void Start()
    {
        // Tìm ObjectPool trong scene
        objectPool = FindObjectOfType<ObjectPool>();
        if (objectPool == null)
        {
            Debug.LogError("ObjectPool not found in the scene. Ensure there is an ObjectPool component in the scene.");
        }
    }

    private void Update()
    {
        // Kiểm tra đầu vào để bắn
        if (Input.GetKeyDown(KeyCode.E) && CanFire())
        {
            Fire();
        }
    }

    private bool CanFire()
    {
        // Kiểm tra thời gian giữa các lần bắn
        return Time.time >= lastFireTime + fireRate;
    }

    private void Fire()
    {
        lastFireTime = Time.time;

        // Kích hoạt animation bắn
        playerController?.animator.SetTrigger("RangedAttack");

        PlayFireSound();
        SpawnBullet();
    }

    private void PlayFireSound()
    {
        // Chơi âm thanh bắn
        if (AudioManager.Instance != null && AudioManager.Instance.bowClip != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.bowClip);
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance or bowClip is missing. Ensure they are properly assigned.");
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
                // Xác định hướng của người chơi
                bool isFacingRight = playerController.isFacingRight;
                float direction = isFacingRight ? 1f : -1f;

                // Đặt vị trí và kích thước cho đạn
                bullet.transform.position = firePoint.position;

                // Điều chỉnh hướng và tốc độ của viên đạn
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                BulletController bulletController = bullet.GetComponent<BulletController>();

                if (bulletRb != null && bulletController != null)
                {
                    // Đặt hướng và tốc độ cho viên đạn
                    bulletRb.velocity = direction * firePoint.right * bulletController.speed;

                    // Nếu cần, có thể thiết lập góc quay của đạn
                    bullet.transform.rotation = Quaternion.Euler(0, 0, isFacingRight ? 0 : 180);
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
