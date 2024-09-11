using UnityEngine;
using TMPro;

public class PlayerCollisionHandling : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreUI;
    public Animator animator;

    private void Awake()
    {
        // this.audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()

    {
        this.scoreUI.SetText(this.score.ToString());
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Kiểm tra nếu đối tượng va chạm là táo
        if (collider.CompareTag("Apple"))
        {
            // Tăng điểm
            this.score += 1;
            this.scoreUI.SetText(this.score.ToString());
            AudioManager.Instance.PlaySFX(AudioManager.Instance.eatClip);
            this.animator.SetTrigger("Eat");
            // Xóa táo khỏi cảnh
            Destroy(collider.gameObject);
        }
    }
}
