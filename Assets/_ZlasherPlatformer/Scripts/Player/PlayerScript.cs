using UnityEngine;

public class PlayerMovementScript : MonoBehaviour, IDamageable
{
    public float jumpForce = 5f;
    public float gravityMultiplier = 1f;
    public float walkSpeed = 2f;
    public float maxWalkSpeed = 10f;
    public float backwardDuration = 1f; // Duration to move backward
    public float cameraBorderSize = 0.1f; // Size of the camera border

    private Rigidbody2D rb;
    private Animator animator;
    private Camera mainCamera;
    private float backwardTimer = 0f;
    private bool moveBackward = false;
    public AnimationClip runAnimationClip; // Reference to the Run animation clip
    public AnimationClip walkAnimationClip; // Reference to the Walk animation clip

    [SerializeField] private ContactFilter2D groundFilter;
    public bool IsGrounded => rb.IsTouching(groundFilter);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        // Play the Run animation when the game starts
        if (animator != null)
        {
            animator.SetTrigger("Run");
        }
    }

    void Update()
    {
        float horizontalInput = 0f;

        // Adjust walk speed based on input
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
            moveBackward = false; // Reset the backward movement flag
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
            moveBackward = false; // Reset the backward movement flag
        }
        else if (!Input.GetKey(KeyCode.D) && moveBackward)
        {
            backwardTimer += Time.deltaTime;
            if (backwardTimer <= backwardDuration)
            {
                horizontalInput = -1f;
                // Play Walk animation if moving backward
                if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    animator.SetTrigger("Walk");
                }
            }
            else
            {
                moveBackward = false; // Stop moving backward after the duration
                backwardTimer = 0f; // Reset the timer
            }
        }

        // Check if the D key was just released
        if (Input.GetKeyUp(KeyCode.D))
        {
            moveBackward = true;
            backwardTimer = 0f; // Start the backward timer
        }

        // Applying movement
        rb.linearVelocity = new Vector2(horizontalInput * walkSpeed, rb.linearVelocity.y);

        // Applying Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Play Run animation if grounded and not moving backward
        if (IsGrounded && !moveBackward && animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("Run");
        }

        // Check for attack input
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            Debug.Log("Player attacking");
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }

        // Check for attack release input
        if (Input.GetMouseButtonUp(0)) // 0 is the left mouse button
        {
            Debug.Log("Player attack released");
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }

        // Stop the attack animation when it's done playing
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Run");
        }

        // Constrain player within camera view with adjustable border size
        Vector3 playerPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(playerPosition);
        viewportPosition.x = Mathf.Clamp(viewportPosition.x, cameraBorderSize, 1f - cameraBorderSize); // Adjust these values as needed
        transform.position = mainCamera.ViewportToWorldPoint(viewportPosition);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("receive damage: " + damage);
    }
}
