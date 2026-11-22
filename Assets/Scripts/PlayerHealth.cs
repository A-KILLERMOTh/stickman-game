using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
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
    [Header("Shield settings")]
    public bool isBlocking = false;
    [SerializeField] private InputActionReference shieldAction;
    [SerializeField] private float blockDuration = 1f;
    [SerializeField] private float blockCooldown = 0.5f;
    private float blockCooldownTimer = 0f;
    private float blockTimer = 0f;
    [Header("Death settings")]
    [SerializeField] private DeathUI deathUI;
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
        deathUI = FindAnyObjectByType<DeathUI>();

    }
    void Start()
    {
        
    }
    private bool isDead = false;
    public bool IsDead => isDead; // Public getter for isDead
    void Die()
    {
        deathUI.ShowDeath();
        animator.SetTrigger("die");
        isDead = true;
    }
    public void TakeDamage(int damage,float direction,float knockbackFromEnemy)
    {
        if (isDead) return; // Prevent further damage if the player is already dead
        if(isBlocking == false || (isBlocking == true && direction == playerMovement.FacingDirection))
        {
            
            currentHealth -= damage;
            if (currentHealth <= 0)
        {
            Die();
            return;
        } 
            animator.SetTrigger("Hurt");
            playerMovement.ApplyKnockback(direction, knockbackFromEnemy); // Apply knockback in the direction of the enemy
            
        }
        else
        {
            //player blocked the attack with shield.
            playerMovement.ApplyKnockback(direction, knockbackFromEnemy / 2f); 
            // Apply knockback in the same direction with reduced force
            Debug.Log("BLOCKED");
            audioSource.PlayOneShot(BlockHitSound);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(blockCooldownTimer > 0f)
        {
            blockCooldownTimer -= Time.deltaTime;
        }
        if(blockTimer > 0f && isBlocking)
        {
            blockTimer -= Time.deltaTime;
        }
        if(shieldAction.action.triggered && blockCooldownTimer <= 0f)
        {
            isBlocking = true;
            animator.SetBool("isBlocking", true);
            blockTimer = blockDuration;
        }

        if(shieldAction.action.WasReleasedThisFrame() || (isBlocking && blockTimer <= 0f))
        {
            isBlocking = false;
            blockCooldownTimer = blockCooldown;
            blockTimer = 0f; // Reset block timer when shield is released
            animator.SetBool("isBlocking", false);
            
        }

    }
    
}
