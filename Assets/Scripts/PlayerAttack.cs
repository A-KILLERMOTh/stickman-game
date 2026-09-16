using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private InputActionReference attack_1;

    private PlayerMovement playerMovement;

    [SerializeField] private float attackDuration = 0.2f;
    private bool isAttacking;
    private float attackTimer;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackCooldown = 0.3f;
    private float attackCooldownTimer;
    public void DealDamage()
{
    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
        attackPoint.position,
        attackRange,
        enemyLayer
    );

    foreach (Collider2D enemy in hitEnemies)
    {
        enemy.GetComponent<Enemy>().TakeDamage(
            attackDamage,
            playerMovement.FacingDirection
        );
    }

    Debug.Log("ATTACK! Hit " + hitEnemies.Length + " enemies.");
}

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        attack_1.action.Enable();
        
    }

    private void OnDisable()
    {
        attack_1.action.Disable();
    }

    private void Update()
    {
        attackPoint.localPosition = new Vector3(
        Mathf.Abs(attackPoint.localPosition.x) * playerMovement.FacingDirection,
        attackPoint.localPosition.y,
        attackPoint.localPosition.z);
        if (attack_1.action.triggered && !isAttacking && attackCooldownTimer <= 0f)
        {
            isAttacking = true;
            attackTimer = attackDuration;
            attackCooldownTimer = attackCooldown;
            animator.SetTrigger("Attack_1");
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }
        }
        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}