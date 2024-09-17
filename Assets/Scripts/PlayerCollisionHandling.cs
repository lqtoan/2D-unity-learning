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
    [SerializeField] private float maxHp = 3f;
    [SerializeField] private float currentHp;

    private void Awake()
    {
        // this.audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()

    {
        this.scoreUI.SetText(this.score.ToString());
        this.hpBar.value = maxHp;
        currentHp = maxHp;
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
        currentHp -= damage;

        hpBar.value = currentHp / maxHp; // Giả sử max HP là 3

        this.animator.SetTrigger("Hurt");


        AudioManager.Instance.PlaySFX(AudioManager.Instance.hurtClip);


        if (currentHp <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
