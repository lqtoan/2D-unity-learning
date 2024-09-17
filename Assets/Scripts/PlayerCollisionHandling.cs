using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCollisionHandling : MonoBehaviour
{
    public TextMeshProUGUI scoreUI;
    public Slider hpBar;
    public Animator animator;
    private int score = 0;
    [SerializeField] private float hp = 3f;

    private void Awake()
    {
        // this.audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()

    {
        this.scoreUI.SetText(this.score.ToString());
        this.hpBar.value = 3f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Apple"))
        {
            this.score += 1;
            this.scoreUI.SetText(this.score.ToString());

            this.animator.SetTrigger("Eat");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.eatClip);

            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        hpBar.value = hp / 3f; // Giả sử max HP là 3

        // Kích hoạt animation Hurt
        this.animator.SetTrigger("Hurt");


        // AudioManager.Instance.PlaySFX(AudioManager.Instance.hurtClip);


        if (hp <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
