using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private PlayerController playerController;
    private float comboDelay = 0.3f;
    private int comboStep = 0;
    private float lastComboTime = 0f;

    private void Awake()
    {
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        this.HandleCombo();
    }

    void HandleCombo()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.swordClip);
            if (Time.time - this.lastComboTime < this.comboDelay)
            {
                this.comboStep++;
                if (this.comboStep > 3) this.comboStep = 3;
            }
            else
            {
                this.comboStep = 1;
            }

            this.lastComboTime = Time.time;

            switch (this.comboStep)
            {
                case 1:
                    this.playerController.animator.SetTrigger("Attack1");
                    break;
                case 2:
                    this.playerController.animator.SetTrigger("Attack2");
                    break;
                case 3:
                    this.playerController.animator.SetTrigger("Attack3");
                    this.comboStep = 0;
                    break;
            }
        }
        
        if (Time.time - this.lastComboTime > this.comboDelay && this.comboStep > 0)
        {
            this.comboStep = 0;
        }
    }
}
