using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float jumpForce = 5f;
    public float gravityMultiplier = 1f;
    public float walkSpeed = 2f;
    public float maxWalkSpeed = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private Camera mainCamera;

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
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }

        // Applying movement
        rb.linearVelocity = new Vector2(horizontalInput * walkSpeed, rb.linearVelocity.y);

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

        // Constrain player within camera view
        Vector3 playerPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(playerPosition);
        viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.05f, 0.95f); // Adjust these values as needed
        transform.position = mainCamera.ViewportToWorldPoint(viewportPosition);
    }
}
