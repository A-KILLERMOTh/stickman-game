using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private int health = 100;
    [SerializeField] private float knockbackForce = 2f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color orignalColor;
    [SerializeField] public BoxCollider2D EnemyCollider;
    [SerializeField] private float damageCooldown = 1f;
    private float damageCooldownTimer;
    private PlayerHealth playerHealth;
    private float playerKnockbackDirection;
    [SerializeField]private  Transform player;
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
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        orignalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
        EnemyCollider = GetComponent<BoxCollider2D>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        baseGravityScale = rb.gravityScale;
    }
    private void ResetColor()
    {
        spriteRenderer.color = orignalColor;
    }

    public void TakeDamage(int damage,float direction)
    {    // Reverse the direction for knockback
        health -= damage;
        spriteRenderer.color = Color.red;
        Invoke(nameof(ResetColor), 0.1f);

        rb.linearVelocity = new Vector2(knockbackForce * direction, rb.linearVelocity.y);
        

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
    private void GiveDamage()
    {
        playerKnockbackDirection = Mathf.Sign(playerHealth.transform.position.x - transform.position.x); // Determine the direction of knockback based on the relative position of the player and enemy
        playerHealth.TakeDamage(10,playerKnockbackDirection);
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
        if(EnemyCollider.IsTouching(playerHealth.PlayerCollider) && damageCooldownTimer <= 0)
        {
            GiveDamage();
            damageCooldownTimer = damageCooldown;
        }
        if(damageCooldownTimer > 0)
        {
            damageCooldownTimer -= Time.deltaTime;
        }  
    }
    void FixedUpdate()
    {
        HandleGravity();
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
} 