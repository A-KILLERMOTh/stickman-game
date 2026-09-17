using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed = 5f;
    //prevention from infinite jumping requires a ground check to see if the player is on the ground or not
    //its done by creating a small circle(groundCheckRadius) at the bottom of the player and checking if it collides with the ground layer
    [SerializeField] private Transform GroundCheck;
     [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    private bool isGrounded;
    

    [SerializeField] private  float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    [SerializeField] private float fallMultiplier = 2.5f;
    private float orignalGravity;
    private Rigidbody2D rb;
    private Vector2 movement;
    [SerializeField] private float jumpForce;
    [SerializeField] private InputActionReference jumpAction;

    [SerializeField] private InputActionReference dashAction;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private GameObject dashWindPrefab;

    private GameObject dashWindInstance;
    private Vector2 dashStartPosition;
    private SpriteRenderer dashWindRenderer;
    private float dashCooldownTimer;

    private float facingDirection = 1f;
    public float FacingDirection => facingDirection;

    private bool isDashing;
    private float dashTimer;
    private bool hasAirDashed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        orignalGravity = rb.gravityScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    private void OnEnable()
    {
        jumpAction.action.Enable();
        dashAction.action.Enable();
       
    }
    private void OnDisable()
    {
        jumpAction.action.Disable();
        dashAction.action.Disable();
       
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        

        movement.Normalize();
        animator.SetBool("IsMoving", movement.x != 0);

        if (movement.x != 0)
        {
            facingDirection = movement.x;
            spriteRenderer.flipX = facingDirection < 0;
            
        }


        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, groundLayer) != null;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            hasAirDashed = false;
            
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            
        }
        

      if (dashAction.action.triggered && !isDashing && dashCooldownTimer <= 0 && (isGrounded || !hasAirDashed))
        {
            isDashing = true;
            animator.SetBool("isDashing", true);
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            dashStartPosition = transform.position;
            dashWindInstance = Instantiate(dashWindPrefab);
            dashWindRenderer = dashWindInstance.GetComponent<SpriteRenderer>();

            if (!isGrounded)
            {
                hasAirDashed = true;
            }
        }
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if(jumpAction.action.triggered)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,jumpForce);
            Debug.Log("coyote: " + coyoteTimeCounter + " Jump Buffer: " + jumpBufferCounter);
        }
        if (jumpAction.action.WasReleasedThisFrame() && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
        
    }

   private void FixedUpdate()
{
    if (rb.linearVelocity.y < 0)
    {
        rb.gravityScale = orignalGravity;
    }
    else
    {
        rb.gravityScale = orignalGravity * fallMultiplier;
    }

    if (isDashing)
    {
        rb.linearVelocity = new Vector2(
            facingDirection * dashSpeed,
            rb.linearVelocity.y
        );

        if (dashWindInstance != null)
        {
            Vector2 currentPosition = transform.position;

            Vector2 midpoint = (dashStartPosition + currentPosition) / 2f;

            dashWindInstance.transform.position = midpoint;

            float distance = Vector2.Distance(
                dashStartPosition,
                currentPosition
            );

            float originalWidth = 384f / 50f;

            dashWindInstance.transform.localScale = new Vector3(
                distance / originalWidth,
                1f,
                1f
            );

            dashWindRenderer.flipX = facingDirection < 0;
        }

        dashTimer -= Time.fixedDeltaTime;

        if (dashTimer <= 0)
        {
            isDashing = false;
            animator.SetBool("isDashing", false);

            if (dashWindInstance != null)
            {
                Destroy(dashWindInstance);
            }
        }

        return;
    }

    // Normal movement
    rb.linearVelocity = new Vector2(
        movement.x * moveSpeed,
        rb.linearVelocity.y
    );
}
};