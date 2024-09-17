using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;         // Thanh máu hiện tại
    [SerializeField] private Slider lostHealthSlider;     // Thanh hiển thị máu đã mất
    [SerializeField] private Vector3 offset;              // Vị trí thanh máu so với Enemy
    [SerializeField] private Transform enemyTransform;    // Vị trí của Enemy
    [SerializeField] private Image fillImage;             // Thành phần hình ảnh của thanh máu hiện tại
    [SerializeField] private Image lostHealthFillImage;   // Thành phần hình ảnh của thanh máu đã mất

    // [SerializeField] private Color fullHealthColor = Color.green;
    // [SerializeField] private Color halfHealthColor = Color.yellow;
    // [SerializeField] private Color lowHealthColor = Color.red;

    void Start()
    {
        enemyTransform = transform.parent; // Giả định thanh máu là con của Enemy

        if (healthSlider == null)
        {
            Debug.LogError("HealthSlider is not assigned in the inspector. Please assign it.");
            return; // Dừng thực thi nếu healthSlider không được gán
        }

        ResetHeartBar();
    }

    void Update()
    {
        // Cập nhật vị trí của thanh máu theo vị trí của Enemy
        // transform.position = Camera.main.WorldToScreenPoint(enemyTransform.position + offset);

    }

    // Cập nhật thanh máu khi kẻ thù bị tấn công
    public void SetHealth(float health, float maxHealth)
    {
        if (gameObject.activeInHierarchy)
        {
            float healthPercent = health / maxHealth;
            Debug.Log(healthPercent);
            // Cập nhật ngay lập tức thanh máu hiện tại
            healthSlider.value = healthPercent;

            // Cập nhật màu sắc cho thanh máu hiện tại
            // UpdateHealthBarColor(healthPercent);

            // Thanh máu đã mất giảm sau transitionDuration
            StartCoroutine(UpdateLostHealth(lostHealthSlider.value, healthPercent));
        }
    }

    public void ResetHeartBar() {
        // Cập nhật thanh máu khi bắt đầu
        healthSlider.value = healthSlider.maxValue;
        lostHealthSlider.value = lostHealthSlider.maxValue;
    }

    // Coroutine cập nhật thanh máu đã mất
    private IEnumerator UpdateLostHealth(float startValue, float endValue)
    {
        float elapsedTime = 0f;
        float transitionDuration = 0.2f; 
        startValue = lostHealthSlider.value;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / transitionDuration);
            lostHealthSlider.value = newValue;
            yield return null;
        }

        // Đảm bảo giá trị cuối cùng là đúng
        lostHealthSlider.value = endValue;
    }

    // Cập nhật màu sắc của thanh máu hiện tại
    // private void UpdateHealthBarColor(float healthPercent)
    // {
    //     if (healthPercent >= 0.5f)
    //     {
    //         fillImage.color = Color.Lerp(halfHealthColor, fullHealthColor, (healthPercent - 0.5f) * 2);
    //     }
    //     else
    //     {
    //         fillImage.color = Color.Lerp(lowHealthColor, halfHealthColor, healthPercent * 2);
    //     }
    // }
}
