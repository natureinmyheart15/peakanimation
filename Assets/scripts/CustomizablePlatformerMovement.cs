using UnityEngine;
using UnityEngine.InputSystem;

            /////////////// INFORMATION ///////////////
// This script is intended to work on a GameObject with
// a Rigidbody2D-component (tip: it probably needs constraints),
// a CapsuleCollider2D-component (or other Collider2D-shape that fits your character)
// and a PlayerInput-component.

// Additionally this script also needs to reference a Collider2D to use for ground-check,
// This groundcheck-collider does not need to be placed on the same object, it could instead be placed on a child object.
// This groundcheck-collider should be a trigger-collider.

// Use UnityEvents in the PlayerInput-component to call movement-functions in this script.

// This script has setup for playing animations and sounds.

public class CustomizablePlatformerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;
    
    
    // Physics and collision
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private CircleCollider2D groundCheckCollider;
    [SerializeField] private LayerMask groundLayer = ~0; // ~0 is referring to EVERY layer.
    
    // Handle input
    public bool controlEnabled { get; set; } = true; // You can edit this variable from Unity Events
    private Vector2 moveInput;
    private Vector2 velocity;
    private bool jumpInput;
    private bool jumpReleased;
    private bool wasGrounded;
    private bool isGrounded;

    [Header("Graphics")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        
        rigidbody.gravityScale = 0; // Set gravity scale to 0 so player won't "fall". We will apply our own gravity!

        //animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        //---Handle vertical velocity---
        
        isGrounded = IsGrounded();
        ApplyGravity();
        
        // Apply jump-input:
        if (jumpInput && wasGrounded)
        {
            velocity.y = jumpForce;
            jumpInput = false;
        }
        
        // Check if character lost contact with ground this frame
        if (wasGrounded == true && isGrounded == false)
        {
            if (velocity.y < 0)
            {
                // Has fallen. Play fall sound and/or trigger fall animation etc
            }
            else
            {
                // Has jumped. Play jump sound and/or trigger jump animation etc
            }
        }
        // Check if character gained contact with ground this frame
        else if (wasGrounded == false && isGrounded == true)
        {
            jumpReleased = false;
            // Has landed, play landing sound and trigger landing animation
        }
        
        rigidbody.linearVelocity = velocity;
        wasGrounded = isGrounded;
        
        //---Handle horizontal velocity---
        velocity.x = moveInput.x * maxSpeed;
        
        // Write movement animation code here. (Suggestion: send your current velocity into the Animator for both the x- and y-axis.)
        // animator.SetFloat("HorizontalMovement", moveInput.x)
        
        // Flip sprite according to direction (if a sprite renderer has been assigned)
        if (spriteRenderer)
        {
            if (moveInput.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (moveInput.x < -0.01f)
                spriteRenderer.flipX = true;
        }
    }

    private bool IsGrounded()
    {
        // Is our groundCheckCollider touching the groundLayer? If so, return the value "true"
        if (groundCheckCollider.IsTouchingLayers(groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ApplyGravity()
    {
        // Applies a set gravity for when player is grounded
        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -1.0f;
        }
        // Updates fall speed with gravity if object isn't grounded
        else
        {
            // Is Jumping upwards
            if (velocity.y > 0)
            {
                float deceleration = 1;
                if (jumpReleased) // shorter jump height when releasing the jump-key
                {
                    deceleration = 5;
                }
                // you can add a gravity multiplier here... but how?
                velocity.y += Physics2D.gravity.y * deceleration * Time.deltaTime;
            }
            // Is Falling
            else
            {
                // you can add a gravity multiplier here... but how?
                velocity.y += Physics2D.gravity.y * Time.deltaTime;
            }
        }
    }

    // Handle Move-input
    // This method can be triggered through the UnityEvent in PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        if (controlEnabled)
        {
            moveInput = context.ReadValue<Vector2>().normalized;
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    // Handle Jump-input
    // This method can be triggered through the UnityEvent in PlayerInput
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && controlEnabled)
        {
            Debug.Log("Jump!");
            jumpInput = true;
            jumpReleased = false;
        }

        if (context.canceled && controlEnabled)
        {
            jumpReleased = true;
            jumpInput = false;
        }
    }
}
