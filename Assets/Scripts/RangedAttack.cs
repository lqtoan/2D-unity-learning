using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;

    private PlayerController playerController;
    private float lastFireTime;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found on the Player object. Please ensure the Player object is tagged correctly and has a PlayerController component.");
        }
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
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogWarning("BulletPrefab or FirePoint is not assigned. Please assign them in the Inspector.");
        }
    }
}
