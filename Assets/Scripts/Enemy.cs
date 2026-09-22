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
    
    
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        orignalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
        EnemyCollider = GetComponent<BoxCollider2D>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
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
        
        Debug.Log("Enemy took " + damage + " damage. Health: " + health);

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
} 