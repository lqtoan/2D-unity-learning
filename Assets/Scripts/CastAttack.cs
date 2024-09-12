using UnityEngine;

public class CastAttack : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject spellPrefab; 
    public Transform castPoint; 
    public float castCooldown = 0.5f;

    private float lastCastTime;

    private void Awake()
    {
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= this.lastCastTime + this.castCooldown)
        {
            this.playerController.animator.SetTrigger("Cast");
            this.CastSpell();
        }
    }

    void CastSpell()
    {
        Instantiate(this.spellPrefab, this.castPoint.position, this.castPoint.rotation);
        this.lastCastTime = Time.time;
    }
}
