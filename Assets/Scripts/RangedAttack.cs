using System.Collections;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    private PlayerController playerController;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [SerializeField] float fireRate = 1f;
    private float delayBeforeFire = 0.5f;
    private float lastFireTime;

    private void Awake()
    {
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= this.lastFireTime + this.fireRate)
        {
            this.playerController.animator.SetTrigger("RangedAttack");
            StartCoroutine(this.Fire());
        }
    }

    private IEnumerator Fire()
    {
        this.lastFireTime = Time.time;
        yield return new WaitForSeconds(delayBeforeFire);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.bowClip);

        Instantiate(this.bulletPrefab, this.firePoint.position, this.firePoint.rotation);
    }
}
