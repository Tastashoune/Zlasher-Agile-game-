using UnityEngine;
using MyInterface;
using System.Collections;
public class EnemyPoliceman : MonoBehaviour, IEnemyInterface, IDamageable
{
    [Header("Health setting")]
    public int maxHealth = 50; // Santé maximale de l'ennemi

    [Header("Bullet prefab")]
    public GameObject bullet; // prefab du projectile
    public float bulletOffsetY;
    [Header("Collectable head")]
    public GameObject cHead;

    private AudioManager audioInstance;

    private bool isFirstShoot = true;

    private float timeElapsed;
    [SerializeField]
    private float attackDelay;

    [Header("Self walk (false by default)")]
    public bool selfwalk = false;

    private int currentHealth; // Santé actuelle (initialisée dans Start)
    private EnemyState currentState;
    private Rigidbody2D enemyBody;
    private float moveSpeed;

    private Animator animator; // Reference to the Animator component

    // infos (limites) écran
    private float screenLimitLeft;
    private float screenLimitRight;
    private float screenWidth;
    private float spriteSize;

    [Header("Animation and Shooting Delay")]
    [SerializeField]
    private float animationDelay = 0.5f; // Delay to wait for the animation to play before shooting

    void Start()
    {
        isFirstShoot = true;
        currentHealth = maxHealth; // santé max par défaut
        currentState = EnemyState.Walking; // "marche" par défaut
        moveSpeed = -2.0f;
        enemyBody = GetComponent<Rigidbody2D>();
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // Get the Animator component
        animator = GetComponent<Animator>();

        // limite gauche écran et largeur totale
        Camera mainCamera = Camera.main;
        screenLimitLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        screenLimitRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        screenWidth = screenLimitRight - screenLimitLeft;

        timeElapsed = 0f;

        // récupération de l'instance d'AudioManager
        audioInstance = AudioManager.instance;

    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Walking:
                float currentPosX = transform.position.x;
                if (enemyBody != null && selfwalk)
                {
                    if (currentPosX > screenLimitLeft + spriteSize)
                    {
                        Vector2 direction = new Vector2(moveSpeed, enemyBody.linearVelocity.y);
                        enemyBody.linearVelocity = direction;
                    }
                }

                if (currentPosX < screenLimitLeft)
                    Die();

                if (HasPlayerInFront())
                {
                    currentState = EnemyState.Attacking;
                }
            break;

            case EnemyState.Attacking:
                if (timeElapsed > attackDelay || isFirstShoot)
                {
                    StartCoroutine(PlayAnimationAndShoot());
                    isFirstShoot = false;
                    timeElapsed = 0f;
                }
                timeElapsed += Time.deltaTime;

                if (!HasPlayerInFront())
                {
                    currentState = EnemyState.Walking;
                    isFirstShoot = true;
                }
            break;

            default:
            break;
        }
    }

    public bool HasPlayerInFront()
    {
        Vector2 origine = new Vector2(transform.position.x - spriteSize, transform.position.y);
        return Physics2D.Raycast(origine, Vector2.left, screenWidth, LayerMask.GetMask("Player"));
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            //gameObject.SetActive(false);
            SoundOfDeath();
            DropHead();
            Die();
        }
    }

    private IEnumerator PlayAnimationAndShoot()
    {
        // Play the e_Policeman animation
        if (animator != null)
        {
            animator.SetTrigger("e_Policeman");
        }

        // Wait for the animation to finish before shooting
        yield return new WaitForSeconds(animationDelay);

        // Shoot the projectile
        Vector3 bulletPosition = new Vector3(transform.position.x - spriteSize, transform.position.y + bulletOffsetY, transform.position.z);
        Instantiate(bullet, bulletPosition, bullet.transform.rotation);
    }

    public void Fly()
    {
        // le policeman ne fly pas
    }

    public void DropHead()
    {
        // pop de la tête collectable (bonus point de vie)
        // en haut de l'ennemi pour avoir le temps de la collecter
        Vector3 headPosition = new Vector3(transform.position.x - spriteSize, transform.position.y + spriteSize * 3);
        Instantiate(cHead, headPosition, cHead.transform.rotation);
    }
    public void SoundOfDeath()
    {
        // son de mort de l'ennemi
        if (audioInstance != null)
        {
            Debug.Log("audio OK");
            audioInstance.audioSource.clip = audioInstance.playlist[(int)AudioManager.Sounds.EnemyKill];
            audioInstance.audioSource.Play();
        }
    }
    public void Die()
    {
        // Notify the score system
        GetComponent<EnemyDeathNotifier>()?.NotifyDeath();

        // Play enemy death sound
        if (audioInstance != null)
        {
            Debug.Log("audio OK");
            audioInstance.audioSource.clip = audioInstance.playlist[(int)AudioManager.Sounds.EnemyKill];
            audioInstance.audioSource.Play();
        }

        // Object pooling or repositioning logic
        float screenLimitTop = 0f;
        transform.position = new Vector3(screenLimitRight, screenLimitTop);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (selfwalk)
            return;

        if (collision.gameObject.CompareTag("Floor"))
        {
            transform.SetParent(collision.gameObject.transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 origine = new Vector2(transform.position.x - spriteSize, transform.position.y);
        Debug.DrawLine(origine, Vector2.up * origine + Vector2.left * screenWidth);
    }

    public void Shoot()
    {
        throw new System.NotImplementedException();
    }
}
