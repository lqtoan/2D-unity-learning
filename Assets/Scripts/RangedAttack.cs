using System.Collections;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.5f;

    private float delayBeforeFire = 0.5f; // Thời gian delay trước khi bắn viên đạn
    private float lastShootTime;

    private void Awake()
    {
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= this.lastShootTime + this.shootCooldown)
        {
            this.playerController.animator.SetTrigger("RangedAttack");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.castClip);

            StartCoroutine(this.Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(delayBeforeFire);
        Instantiate(this.bulletPrefab, this.shootPoint.position, this.shootPoint.rotation);
        this.lastShootTime = Time.time;
    }
}
