using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] private float enragedThreshold = 0.75f;
    [SerializeField] private float enragedSpeedMultiplier = 1.5f;
    [SerializeField] private GameObject specialAttackPrefab;
    [SerializeField] private float specialAttackCooldown = 1f;

    private bool isEnraged = false;
    private bool canUseSpecialAttack = true;
    private List<GameObject> activeSpecialAttacks = new List<GameObject>();

    private new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();

        // Kiểm tra trạng thái tức giận
        if (!isEnraged && currentHealth < maxHealth * enragedThreshold)
        {
            EnterEnragedState();
        }

        // Thực hiện đòn tấn công đặc biệt khi có thể
        if (isEnraged && canUseSpecialAttack && specialAttackPrefab != null)
        {
            StartCoroutine(SpecialAttack());
        }

        Move();
    }

    private new void Move()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float playerPositionX = player.transform.position.x;
            float bossPositionX = transform.position.x;

            // Chỉ tính direction theo trục X
            float directionX = Mathf.Sign(playerPositionX - bossPositionX);

            float distanceX = Mathf.Abs(playerPositionX - transform.position.x);
            float stopDistance = 4f;

            if (distanceX > stopDistance)
            {
                rb.velocity = new Vector2(directionX * speed, rb.velocity.y);

                if (directionX > 0 && !base.isFacingRight)
                {
                    base.FlipDirection();
                }
                else if (directionX < 0 && base.isFacingRight)
                {
                    base.FlipDirection();
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
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
            activeSpecialAttacks.Add(specialAttack);

            Rigidbody2D rb = specialAttack.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                specialAttack.transform.position = transform.position;
                specialAttack.transform.rotation = Quaternion.identity;

                Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
                rb.velocity = direction * 5f;
            }

            yield return new WaitForSeconds(2f);
            activeSpecialAttacks.Remove(specialAttack);
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

        foreach (var attack in activeSpecialAttacks)
        {
            base.objectPool.ReturnObject(attack); // Trả về object pool
        }
        activeSpecialAttacks.Clear();

        gameObject.SetActive(false);
    }

    private void DropLoot()
    {
#if UNITY_EDITOR
        Debug.Log("Boss has been defeated! Dropping loot.");
#endif
    }
}
