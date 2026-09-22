using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private InputActionReference attackAction;
    public bool IsAttacking => isAttacking;

    private PlayerMovement playerMovement;
    private bool isAttacking;
    private int comboStep = 0;
    private bool attackQueued = false;
    private bool canQueueNextAttack = false;

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


}
    private void StartAttack()
{
    isAttacking = true;

    comboStep++;

    if (comboStep > 3)
        comboStep = 1;

    
    canQueueNextAttack = false;
    attackQueued = false;
    animator.SetInteger("AttackIndex", comboStep);
    animator.SetTrigger("Attack");

    attackQueued = false;
}
    public void OpenComboWindow()
{
    canQueueNextAttack = true;
}

    public void CloseComboWindow()
{
    canQueueNextAttack = false;
}
    public void AttackFinished()
{
    
    if (attackQueued && comboStep < 3)
    {
        StartAttack();
    }
    else
    {
        isAttacking = false;
        comboStep = 0;
        attackQueued = false;
        attackCooldownTimer = attackCooldown;
        canQueueNextAttack = false;
        animator.SetInteger("AttackIndex",0);
    }
}


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
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
        if (attackAction.action.triggered)
        {   
            if (!isAttacking && attackCooldownTimer <= 0f)
            {
                StartAttack();
            }
            else if (isAttacking && canQueueNextAttack)
            {
                attackQueued = true;
            }
        }
        if(attackCooldownTimer > 0f)
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