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

    private Rigidbody2D rb;
    private Vector2 movement;
    [SerializeField] private float jumpForce;
    [SerializeField] private InputActionReference jumpAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
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

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, groundLayer);

        if (jumpAction.action.triggered && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,jumpForce);
            Debug.Log("Jump");
        }
        if (jumpAction.action.WasReleasedThisFrame() && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
        
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2 (movement.x * moveSpeed,rb.linearVelocity.y);
    }
}