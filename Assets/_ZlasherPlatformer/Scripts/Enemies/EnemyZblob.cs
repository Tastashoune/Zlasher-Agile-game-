using UnityEngine;
using MyInterface;
public class EnemyZblob : MonoBehaviour, IEnemyInterface, IDamageable
{
    [Header("Health setting")]
    public int maxHealth = 5; // Maximum health of the enemy

    [Header("Self walk (false by default)")]
    public bool selfwalk = false;

    private int currentHealth; // Current health of the enemy
    private EnemyState currentState;

    private Rigidbody2D enemyBody;
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;
    private Animator animator; // Reference to the Animator component

    // Movement with the ground
    private GameObject levelGenerator;
    private Vector3 groundLastPosition;
    private bool isOnGround = false; // Tracks if the Zblob is on the ground

    void Start()
    {
        // Initialize enemy properties
        currentHealth = maxHealth;
        currentState = EnemyState.Walking;
        enemyBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        // Ensure the Animator component exists
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on EnemyZblob!");
        }

        // Find the LevelGenerator object
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator");
        if (levelGenerator != null)
        {
            groundLastPosition = levelGenerator.transform.position;
        }
        else
        {
            Debug.LogError("LevelGenerator object not found! Ensure it has the correct tag.");
        }

        // Start with the BlobDrop animation
        if (animator != null)
        {
            animator.SetTrigger("BlobDrop");
        }
    }

    void Update()
    {
        if (isOnGround && levelGenerator != null)
        {
            // Move with the LevelGenerator
            Vector3 groundDelta = levelGenerator.transform.position - groundLastPosition;
            transform.position += groundDelta;
            groundLastPosition = levelGenerator.transform.position;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Attach to the LevelGenerator when it touches it
        if (collision.gameObject.CompareTag("Floor") && !isOnGround)
        {
            Debug.Log("Zblob touched the Floor!");
            isOnGround = true;
            enemyBody.isKinematic = true; // Stop physics-based movement
            enemyBody.linearVelocity = Vector2.zero; // Stop any remaining velocity
            groundLastPosition = levelGenerator.transform.position; // Sync with LevelGenerator's position

            // Play the BlobFlat animation
            if (animator != null)
            {
                animator.SetTrigger("BlobFlat");
                Debug.Log("Flatten animation triggered.");
            }
            Debug.Log(collision.gameObject);
            transform.SetParent(collision.gameObject.transform);
        }

        // Deal 50% of the player's current health as damage
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<IDamageable>();
            if (player != null)
            {
                int playerCurrentHealth = player.getCurrentHealth();
                int damage = Mathf.CeilToInt(playerCurrentHealth * 0.5f); // Calculate 50% damage
                Debug.Log($"Player hit by Zblob! Dealing {damage} damage.");
                player.TakeDamage(damage);
                Die();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Detach from the LevelGenerator when leaving it
        if (collision.gameObject.CompareTag("LevelGenerator") && isOnGround)
        {
            isOnGround = false;
            enemyBody.isKinematic = false; // Re-enable physics-based movement
        }
    }

    private void OnBecameInvisible()
    {
        // Destroy the Zblob when it goes off-screen
        Destroy(gameObject);
    }

    public void Shoot()
    {
        // Zblob does not shoot
    }

    public void Fly()
    {
        // Zblob does not fly
    }

    public void Die()
    {
        // Notify the score system
        GetComponent<EnemyDeathNotifier>()?.NotifyDeath();

        // Destroy the Zblob
        Destroy(gameObject);
    }

    public void DropHead()
    {
        // Zblob does not drop a head
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }
}
