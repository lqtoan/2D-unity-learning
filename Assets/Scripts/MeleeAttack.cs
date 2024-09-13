using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] private float comboDelay = 0.3f;
    [SerializeField] private int maxComboStep = 3;

    private PlayerController playerController;
    private int comboStep = 0;
    private float lastComboTime = 0f;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        HandleCombo();
    }

    private void HandleCombo()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAttackSound();
            UpdateComboStep();
            TriggerComboAnimation();
        }

        ResetComboIfNeeded();
    }

    private void PlayAttackSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.swordClip);
        }
    }

    private void UpdateComboStep()
    {
        if (Time.time - lastComboTime < comboDelay)
        {
            comboStep = Mathf.Min(comboStep + 1, maxComboStep);
        }
        else
        {
            comboStep = 1;
        }

        lastComboTime = Time.time;
    }

    private void TriggerComboAnimation()
    {
        if (playerController != null && playerController.animator != null)
        {
            switch (comboStep)
            {
                case 1:
                    playerController.animator.SetTrigger("Attack1");
                    break;
                case 2:
                    playerController.animator.SetTrigger("Attack2");
                    break;
                case 3:
                    playerController.animator.SetTrigger("Attack3");
                    comboStep = 0; // Optionally reset combo after the third attack
                    break;
            }
        }
    }

    private void ResetComboIfNeeded()
    {
        if (Time.time - lastComboTime > comboDelay && comboStep > 0)
        {
            comboStep = 0;
        }
    }
}
