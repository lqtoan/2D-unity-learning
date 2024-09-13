using System.Collections;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float delayBeforeFire = 0.5f;

    private PlayerController playerController;
    private float lastFireTime;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanFire())
        {
            PerformRangedAttack();
        }
    }

    private bool CanFire()
    {
        return Time.time >= lastFireTime + fireRate;
    }

    private void PerformRangedAttack()
    {
        playerController.animator.SetTrigger("RangedAttack");
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        lastFireTime = Time.time;
        yield return new WaitForSeconds(delayBeforeFire);
        
        PlayFireSound();
        SpawnBullet();
    }

    private void PlayFireSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.bowClip);
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance is null.");
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
            Debug.LogWarning("Bullet Prefab or Fire Point is not assigned.");
        }
    }
}
