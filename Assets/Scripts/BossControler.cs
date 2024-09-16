using System.Collections;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] private float enragedThreshold = 0.75f; // Boss sẽ chuyển sang trạng thái tức giận khi máu dưới 50%
    [SerializeField] private float enragedSpeedMultiplier = 2f; // Tốc độ của Boss khi ở trạng thái tức giận
    [SerializeField] private GameObject specialAttackPrefab; // Prefab cho đòn tấn công đặc biệt của boss
    [SerializeField] private float specialAttackCooldown = 5f; // Thời gian giữa các lần sử dụng đòn tấn công đặc biệt
    private bool isEnraged = false;
    private bool canUseSpecialAttack = true;

    // Gọi hàm base Start() từ EnemyController và thêm logic cho Boss
    private new void Start()
    {
        base.Start();

        if (specialAttackPrefab == null) return;
    }

    private new void Update()
    {
        base.Update();

        // Kiểm tra nếu Boss chuyển sang trạng thái tức giận
        if (!isEnraged && currentHealth < maxHealth * enragedThreshold)
        {
            EnterEnragedState();
        }

        // Thực hiện tấn công đặc biệt nếu có thể
        if (canUseSpecialAttack)
        {
            StartCoroutine(SpecialAttack());
        }
    }

    // Thêm trạng thái tức giận cho boss
    private void EnterEnragedState()
    {
        isEnraged = true;
        speed *= enragedSpeedMultiplier; // Tăng tốc độ
        Debug.Log("Boss is enraged! Increased speed!");
        // animator.SetTrigger("Enraged"); // Kích hoạt animation tức giận (nếu có)
    }

    // Tạo đòn tấn công đặc biệt
    private IEnumerator SpecialAttack()
    {
        canUseSpecialAttack = false;

        // Tạo đòn tấn công tại vị trí của boss
        if (specialAttackPrefab) Instantiate(specialAttackPrefab, transform.position, Quaternion.identity);
        // animator.SetTrigger("SpecialAttack"); // Kích hoạt animation đòn tấn công đặc biệt

        yield return new WaitForSeconds(specialAttackCooldown); // Chờ thời gian hồi chiêu
        canUseSpecialAttack = true; // Cho phép sử dụng lại đòn tấn công đặc biệt
    }

    // Ghi đè phương thức OnTriggerEnter2D để xử lý khi boss nhận sát thương
    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Gọi hàm xử lý cơ bản từ EnemyController

        if (currentHealth == 0)
        {
            // Khi boss chết, có thể thêm logic như rơi ra vật phẩm
            DropLoot();
        }
    }

    // Phương thức rơi vật phẩm khi boss chết
    private void DropLoot()
    {
        Debug.Log("Boss has been defeated! Dropping loot.");
        // Logic để rơi ra vật phẩm (nếu có)
    }
}
