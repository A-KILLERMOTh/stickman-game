using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    //prevention from infinite jumping requires a ground check to see if the player is on the ground or not
    //its done by creating a small circle(groundCheckRadius) at the bottom of the player and checking if it collides with the ground layer
    [SerializeField] private Transform GroundCheck;
     [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        orignalGravity = rb.gravityScale;
        
    }

    private void OnEnable()
    {
        jumpAction.action.Enable();
    }
    private void OnDisable()
    {
        jumpAction.action.Disable();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        

        movement.Normalize();

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, groundLayer) != null;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            
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
        rb.linearVelocity = new Vector2 (movement.x * moveSpeed,rb.linearVelocity.y);

         if (rb.linearVelocity.y < 0)
    {
        rb.gravityScale = orignalGravity;
    }
    else
    {
        rb.gravityScale = orignalGravity * fallMultiplier;
    }
    }
}