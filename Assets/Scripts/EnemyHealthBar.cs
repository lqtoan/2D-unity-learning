using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;  // Sử dụng Slider UI để làm thanh máu
    [SerializeField] private Vector3 offset;       // Vị trí thanh máu so với Enemy

    [SerializeField] private Transform enemyTransform; // Vị trí của Enemy

    void Start()
    {
        enemyTransform = transform.parent; // Giả định rằng thanh máu là con của Enemy
    }

    void Update()
    {
        // Cập nhật vị trí của thanh máu theo vị trí của Enemy
        // transform.position = Camera.main.WorldToScreenPoint(enemyTransform.position + offset);
    }

    public void SetHealth(float health, float maxHealth)
    {
        healthSlider.value = health / maxHealth;
    }
}
