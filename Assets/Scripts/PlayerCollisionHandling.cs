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

        if (collider.CompareTag("Apple"))
        {

            this.score += 1;
            this.scoreUI.SetText(this.score.ToString());

            this.animator.SetTrigger("Eat");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.eatClip);
 
            Destroy(collider.gameObject);
        }
    }
}
