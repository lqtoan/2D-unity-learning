using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public PlayerController playerController;
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
            if (Time.time - lastComboTime < comboDelay)
            {
                comboStep++;
                if (comboStep > 3) comboStep = 3;
            }
            else
            {
                comboStep = 1;
            }

            lastComboTime = Time.time;

            switch (comboStep)
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
        
        if (Time.time - lastComboTime > comboDelay && comboStep > 0)
        {
            comboStep = 0;
        }
    }
}
