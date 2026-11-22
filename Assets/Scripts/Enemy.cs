using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private int health = 100;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color orignalColor;
    [SerializeField] public BoxCollider2D EnemyCollider;
    [SerializeField] private float attackCooldown = 1f;
    private float attackCooldownTimer;
    private PlayerHealth playerHealth;
    private float playerKnockbackDirection;
    [SerializeField]private  Transform player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private float moveSpeed = 2f;
    
    //obstacle detection
    [SerializeField] private LayerMask jumpableLayer;
    [SerializeField] private float obstacleDetectionDistance = 1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform obstacleCheck;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float maxFallSpeed = 15f;

    private float baseGravityScale;
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    private int attackIndex = 1;
    private bool isAttacking = false;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackHitRadius = 1f;
     [SerializeField] private int attack1Damage = 10;
    [SerializeField] private int attack2Damage = 15;
    [SerializeField] private int attack3Damage = 25;
    [SerializeField] private int attack1Knockback = 3;
    [SerializeField] private int attack2Knockback = 5;
    [SerializeField] private int attack3Knockback = 8;

    [Header("hitStop Settings")]
    [SerializeField] private float hitStopDuration1 = 0.1f;
     // Duration of the hit stop effect in seconds
    [SerializeField] private float hitStopDuration2 = 0.15f; // Duration of the hit stop effect in seconds
    [SerializeField] private float hitStopDuration3 = 0.2f;
     
     [Header("Hitstop from Player Attacks")]
    [SerializeField] private float hitStopDurationFromPlayerAttack1 = 0.1f; // Duration of the hit stop effect in seconds
    [SerializeField] private float hitStopDurationFromPlayerAttack2 = 0.15f; // Duration of the hit stop effect in seconds
    [SerializeField] private float hitStopDurationFromPlayerAttack3 = 0.2f;
    [Header("Death Settings")]
    [SerializeField] private float deathDelay = 5f; // Delay before destroying the enemy after death
    private PlayerAttack playerAttack;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        orignalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
        EnemyCollider = GetComponent<BoxCollider2D>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        baseGravityScale = rb.gravityScale;
        playerAttack = player.GetComponent<PlayerAttack>();
    }
    private void ResetColor()
    {
        spriteRenderer.color = orignalColor;
    }

    public void TakeDamage(int damage,float direction,float knockback)
    {   
        if(playerHealth.IsDead)
        {
            return;
        }
      
        health -= damage;
        if (health <= 0)
        {
            animator.SetTrigger("die");
            Die();
            return;
        }
         // Reverse the direction for knockback
        
        spriteRenderer.color = Color.red;
        Invoke(nameof(ResetColor), 0.1f);
        if(isAttacking)
        {
            finishCombo();
        }
        animator.SetTrigger("hit");
        if(playerAttack.comboStep == 1)
        {
            hitStop.instance.StopTime(hitStopDurationFromPlayerAttack1);
        }
        else if(playerAttack.comboStep == 2)
        {
            hitStop.instance.StopTime(hitStopDurationFromPlayerAttack2);
        }
        else if(playerAttack.comboStep == 3)
        {
            hitStop.instance.StopTime(hitStopDurationFromPlayerAttack3);
        }
        
        
        rb.linearVelocity = new Vector2(knockback * direction, rb.linearVelocity.y);
        

    
    }
    private IEnumerator destroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void Die()
    {
       StartCoroutine(destroyAfterDelay(deathDelay));
    }
    public void GiveDamage(int damage,int knockbackFromEnemy)
    {
        
        playerKnockbackDirection = Mathf.Sign(playerHealth.transform.position.x - transform.position.x);
        
         // Determine the direction of knockback based on the relative position of the player and enemy
        playerHealth.TakeDamage(damage,playerKnockbackDirection,knockbackFromEnemy);
    }
    private void startCombo()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        spriteRenderer.flipX = direction < 0;
        isAttacking = true;
        attackIndex = 1;
        animator.SetInteger("attackIndex", attackIndex);
        animator.SetBool("isAttacking", true);
        animator.SetTrigger("attack");
    }
    public void finishCombo()
    {

        isAttacking = false;
        attackIndex = 1;
        animator.SetInteger("attackIndex", attackIndex);
        animator.SetBool("isAttacking", false);
        attackCooldownTimer = attackCooldown;
    
        
    }
    public void continueCombo()
    {
        float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);
        if(distanceToPlayer > attackRange || attackIndex >= 3)
        {
            finishCombo();
            return;
        }
        else
        {
            attackIndex++;
            animator.SetInteger("attackIndex", attackIndex);
            
        }
    }
        private bool PlayerDetected()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position,
        attackHitRadius,playerLayer);
        return hitPlayers.Length > 0;
    }

    public void Attack1Hit()
    {
        if(PlayerDetected() )
        {
            if(playerHealth.isBlocking == false){
            hitStop.instance.StopTime(hitStopDuration1);
            }
            GiveDamage(attack1Damage, attack1Knockback);

        }
    }

    public void Attack2Hit()
    {
        if(PlayerDetected())
        {
            if(playerHealth.isBlocking == false){
            hitStop.instance.StopTime(hitStopDuration2);
            }
            GiveDamage(attack2Damage, attack2Knockback);
        }
    }

    public void Attack3Hit()
    {
        if(PlayerDetected())
        {
            if(playerHealth.isBlocking == false){
            hitStop.instance.StopTime(hitStopDuration3);
            }
            GiveDamage(attack3Damage, attack3Knockback);
            
        }
    }
    private bool IsGrounded()
    {
    return Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
    }
    private bool IsObstacleAhead(float direction)
    {
    Vector2 rayDirection = new Vector2(direction, 0f);

    Vector2 origin = new Vector2(
        EnemyCollider.bounds.center.x +
        direction * (EnemyCollider.bounds.extents.x + 0.05f),
        obstacleCheck.position.y
    );

    Debug.DrawRay(
        origin,
        rayDirection * obstacleDetectionDistance,
        Color.red
    );

    RaycastHit2D hit = Physics2D.Raycast(
        origin,
        rayDirection,
        obstacleDetectionDistance,
        jumpableLayer
    );

    return hit.collider != null;
    }

    private void Jump()
    {
    float gravity = Mathf.Abs(
        Physics2D.gravity.y * baseGravityScale
    );

    float jumpVelocity = Mathf.Sqrt(2f * gravity * jumpHeight);
    

    rb.linearVelocity = new Vector2(
        rb.linearVelocity.x,
        jumpVelocity
    );
    }

private void HandleGravity()
    {
    if (rb.linearVelocity.y < 0f)
    {
        rb.gravityScale = baseGravityScale * fallMultiplier;

        if (rb.linearVelocity.y < -maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                -maxFallSpeed
            );
        }
    }
    else
    {
        rb.gravityScale = baseGravityScale;
    }
    }
    void Update()
    { 
        if(playerHealth.IsDead)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        attackPoint.localPosition = new Vector3(
        Mathf.Abs(attackPoint.localPosition.x) * Mathf.Sign(player.position.x - transform.position.x),
        attackPoint.localPosition.y,
        attackPoint.localPosition.z);
        

        
        if(attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        
        float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);
        if(distanceToPlayer <= attackRange && !isAttacking && attackCooldownTimer <= 0)
        {
            startCombo();
        }
    }
    void FixedUpdate()
    {
        HandleGravity();

        if(isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            
        }

     else{   
        float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);
        if(distanceToPlayer <= detectionRange){
            float direction = Mathf.Sign(player.position.x - transform.position.x);
                spriteRenderer.flipX = direction < 0;
            if(distanceToPlayer > stoppingDistance)
            {
                 // Flip the sprite based on the direction
                rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
                if(IsObstacleAhead(direction) && IsGrounded())
                {
                    Jump();
                }

            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            } 
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
        animator.SetBool("isGrounded", IsGrounded());

        animator.SetFloat("yVelocity", rb.linearVelocity.y);

        animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
    }
      private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackHitRadius);
    }
} 
