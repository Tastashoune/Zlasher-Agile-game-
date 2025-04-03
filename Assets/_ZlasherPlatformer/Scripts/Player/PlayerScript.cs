using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float jumpForce = 5f;
    public float gravityMultiplier = 1f;
    public float walkSpeed = 2f;
    public float acceleration = 0.1f;
    public float maxWalkSpeed = 10f;

    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private ContactFilter2D groundFilter;
    public bool IsGrounded => rb.IsTouching(groundFilter);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Play the Run animation when the game starts
        if (animator != null)
        {
            animator.SetTrigger("Run");
        }
    }

    void Update()
    {
        // Accelerate walk speed over time
        walkSpeed = Mathf.Min(walkSpeed + acceleration * Time.deltaTime, maxWalkSpeed);

        // Adjust walk speed based on input
        if (Input.GetKey(KeyCode.A))
        {
            walkSpeed = Mathf.Max(walkSpeed - acceleration * Time.deltaTime, 0f); // Slow down but don't go backward
        }

        // Applying constant rightward movement
        rb.linearVelocity = new Vector2(walkSpeed, rb.linearVelocity.y);

        // Applying Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Play Run animation if grounded
        if (IsGrounded && animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
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
    }
}
