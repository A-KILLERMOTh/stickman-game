using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private InputActionReference attackAction;
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

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        attackAction.action.Enable();
        
    }

    private void OnDisable()
    {
        attackAction.action.Disable();
    }

    private void Update()
    {
        attackPoint.localPosition = new Vector3(
        Mathf.Abs(attackPoint.localPosition.x) * playerMovement.FacingDirection,
        attackPoint.localPosition.y,
        attackPoint.localPosition.z);
        if (attackAction.action.triggered && !isAttacking && attackCooldownTimer <= 0f)
        {
            isAttacking = true;
            attackTimer = attackDuration;
            attackCooldownTimer = attackCooldown;

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange,
                enemyLayer
            );
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage, playerMovement.FacingDirection);
            }

            Debug.Log("ATTACK! Hit " + hitEnemies.Length + " enemies.");
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