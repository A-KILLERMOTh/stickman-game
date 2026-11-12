using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Animator animator;
    [SerializeField] private  BoxCollider2D playerCollider;
    //this is a getter so that the Enemy script can access the player's collider to check for collisions but cant make changes to it
    public BoxCollider2D PlayerCollider => playerCollider;
    
    private PlayerMovement playerMovement;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip BlockHitSound;
    
    private int currentHealth;
    private bool isBlocking = false;
    [SerializeField] private InputActionReference shieldAction;

    void OnEnable()
    {
        shieldAction.action.Enable();
    }
    void OnDisable()
    {
        shieldAction.action.Disable();
    }
    void Awake()
    {
        
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponentInChildren<AudioSource>();

    }
    void Start()
    {
        
    }
    void Die()
    {
        // Handle player death (e.g., reload scene, show game over screen, etc.)
        Debug.Log("Player has died.");
    }
    public void TakeDamage(int damage,float direction,float knockbackFromEnemy)
    {
        if(isBlocking == false)
        {   
            currentHealth -= damage;

            animator.SetTrigger("Hurt");

            Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
            
            playerMovement.ApplyKnockback(direction, knockbackFromEnemy); // Apply knockback in the direction of the enemy
            
        }
        else
        {
            //player blocked the attack with shield.
            playerMovement.ApplyKnockback(direction, knockbackFromEnemy / 2f); 
            // Apply knockback in the same direction with reduced force
            animator.SetBool("isBlocking", false);
            audioSource.PlayOneShot(BlockHitSound);

        }
        
        if (currentHealth <= 0)
        {
            Die();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(shieldAction.action.triggered)
        {
            isBlocking = true;
            Debug.Log("Player is blocking.");
            animator.SetBool("isBlocking", true);
            
        }
        if(shieldAction.action.WasReleasedThisFrame())
        {
            isBlocking = false;
            Debug.Log("Player stopped blocking.");
            animator.SetBool("isBlocking", false);
            
        }

    }
}
