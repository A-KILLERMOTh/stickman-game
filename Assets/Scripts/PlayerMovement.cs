using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        

        movement.Normalize();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2 (movement.x * moveSpeed,0);
    }
}