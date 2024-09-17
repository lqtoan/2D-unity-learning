using System.Collections;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] private float enragedThreshold = 0.75f;
    [SerializeField] private float enragedSpeedMultiplier = 2f;
    [SerializeField] private GameObject specialAttackPrefab;
    [SerializeField] private float specialAttackCooldown = 1f;

    private bool isEnraged = false;
    private bool canUseSpecialAttack = true;

    private new void Start()
    {
        base.Start();

        if (specialAttackPrefab == null)
        {
            Debug.LogWarning("SpecialAttackPrefab is not assigned.");
        }
    }

    private new void Update()
    {
        base.Update();

        if (!isEnraged && currentHealth < maxHealth * enragedThreshold)
        {
            EnterEnragedState();
        }

        if (!isEnraged && currentHealth < maxHealth * enragedThreshold)
        {
            EnterEnragedState();
        }

        if (isEnraged && canUseSpecialAttack && specialAttackPrefab != null)
        {
            StartCoroutine(SpecialAttack());
        }
    }

    private void EnterEnragedState()
    {
        isEnraged = true;
        speed *= enragedSpeedMultiplier;
        base.animator.SetTrigger("Summon");

        if (canUseSpecialAttack)
        {
            StartCoroutine(SpecialAttack());
        }
    }

    private IEnumerator SpecialAttack()
    {
        canUseSpecialAttack = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector2 playerPosition = player.transform.position;

            // TODO: obj pooling
            GameObject specialAttack = Instantiate(specialAttackPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rb = specialAttack.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
                rb.velocity = direction * 5f; // Tốc độ đạn có thể tùy chỉnh
            }

            // TODO: obj pooling
            Destroy(specialAttack, 2f);
        }

        yield return new WaitForSeconds(specialAttackCooldown);

        canUseSpecialAttack = true;
    }


    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (currentHealth == 0)
        {
            Die();
            DropLoot();
        }
    }

    protected override void Die()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private void DropLoot()
    {
        Debug.Log("Boss has been defeated! Dropping loot.");
    }
}
