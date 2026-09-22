using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Animator animator;
    [SerializeField] private  BoxCollider2D playerCollider;
    //this is a getter so that the Enemy script can access the player's collider to check for collisions but cant make changes to it
    public BoxCollider2D PlayerCollider => playerCollider;
    
    [SerializeField] private float knockbackForce = 5f;
    private PlayerMovement playerMovement;
    
    private int currentHealth;


    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        
        playerMovement = GetComponent<PlayerMovement>();

    }
    void Start()
    {
        
    }
    void Die()
    {
        // Handle player death (e.g., reload scene, show game over screen, etc.)
        Debug.Log("Player has died.");
    }
    public void TakeDamage(int damage,float direction)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
        
        playerMovement.ApplyKnockback(direction, knockbackForce);
        
        
        if (currentHealth <= 0)
        {
            Die();
        }

    }

    // Update is called once per frame
    void Update()
    {


    }
}
