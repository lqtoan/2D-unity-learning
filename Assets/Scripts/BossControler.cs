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

            GameObject specialAttack = base.objectPool.GetObject(specialAttackPrefab);

            Rigidbody2D rb = specialAttack.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                specialAttack.transform.position = transform.position;
                specialAttack.transform.rotation = Quaternion.identity;

                Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
                rb.velocity = direction * 5f; // Tốc độ đạn có thể tùy chỉnh
            }

            yield return new WaitForSeconds(2f);
            base.objectPool.ReturnObject(specialAttack);
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
        #if UNITY_EDITOR
        Debug.Log("Boss has been defeated! Dropping loot.");
        #endif
    }
}
