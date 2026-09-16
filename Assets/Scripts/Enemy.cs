using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private int health = 100;
    [SerializeField] private float knockbackForce = 2f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color orignalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        orignalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
    }
    private void ResetColor()
    {
        spriteRenderer.color = orignalColor;
    }

    public void TakeDamage(int damage,float direction)
    {
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
}