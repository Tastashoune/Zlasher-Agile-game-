using TMPro;
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

    [SerializeField] private ContactFilter2D groundFilter;
    private float attackTimer;
    [SerializeField] private float attackTimeout = 0.5f;
    private bool isAttacking;

    public bool IsGrounded => rb.IsTouching(groundFilter);
    public int maxHealth = 100;
    private int currentHealth;

    public PlayrHealtBar healthBar; // Reference to the health bar script

    // Lose health over time
    public float healthLossInterval = 1f; // Time interval in seconds to lose health
    public int healthLossAmount = 5; // Amount of health lost per interval
    private float healthLossTimer = 0f;

    [Header("Attack Settings")]
    public GameObject attackRange; // Reference to the child GameObject for attack range
    public int attackDamage = 10; // Damage dealt by the normal attack
    public float attackRangeRadius = 1.5f; // Radius of the attack range
    public ScoreScript scoreScript; // Reference to the ScoreScript

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        // Initialize health
        currentHealth = maxHealth;

        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }

        // Play the Run animation when the game starts
        if (animator != null)
        {
            animator.SetTrigger("Run");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
        ConstrainPlayerWithinCamera();
        HandleHealthLossOverTime();
    }

    private void HandleMovement()
    {
        float horizontalInput = 0f;

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
                if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    animator.SetTrigger("Walk");
                }
            }
            else
            {
                moveBackward = false;
                backwardTimer = 0f; // Reset the timer
            }
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            moveBackward = true;
            backwardTimer = 0f; // Start the backward timer
        }

        rb.linearVelocity = new Vector2(horizontalInput * walkSpeed, rb.linearVelocity.y);

        if (IsGrounded && !moveBackward && animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("Run");
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            isAttacking = true;
            attackTimer = 0f; // Reset the attack timer
        }

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0) && attackTimer < attackTimeout)
        {
            isAttacking = false;
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }

        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Run");
        }
    }

    public void PerformAttack()
    {
        // Get all colliders within the attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRangeRadius, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in hitEnemies)
        {
            // Ensure the detected object is not the player itself
            if (enemy.gameObject == gameObject) continue;

            // Check if the object implements IDamageable
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage); // Deal damage to the enemy
            }
        }
    }

    private void ConstrainPlayerWithinCamera()
    {
        Vector3 playerPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(playerPosition);
        viewportPosition.x = Mathf.Clamp(viewportPosition.x, cameraBorderSize, 1f - cameraBorderSize);
        transform.position = mainCamera.ViewportToWorldPoint(viewportPosition);
    }

    private void HandleHealthLossOverTime()
    {
        healthLossTimer += Time.deltaTime;
        if (healthLossTimer >= healthLossInterval)
        {
            TakeDamage(healthLossAmount);
            healthLossTimer = 0f; // Reset the timer
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //Debug.Log("take dgm");
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead!");

            // Stop the score counter when the player dies
            if (scoreScript != null)
            {
                scoreScript.StopScore();
            }

            gameObject.SetActive(false); // Deactivate the player object
        }
    }

    public void TakeHealth(int healthAmount)
    {
        // take health when you collect an head - Fabien, 14/04/2025
        currentHealth += healthAmount;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

    // Optional: Visualize the attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRangeRadius); // Attack range
    }
}
