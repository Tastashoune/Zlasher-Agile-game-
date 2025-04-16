using UnityEngine;
using MyInterface;
public class EnemyUfo : MonoBehaviour, IEnemyInterface, IDamageable
{
    [Header("Health setting")]
    public float maxHealth = 50f; // Changed to float for compatibility with float damage
    private float currentHealth;  // Changed to float

    //[Header("Player gameobject")]
    private GameObject player;
    [Header("Collectable head")]
    public GameObject cHead;

    private AudioManager audioInstance;

    private EnemyState currentState;
    private Rigidbody2D enemyBody;
    private float moveSpeed;

    // infos (limites) écran
    private float screenLimitLeft;
    private float screenLimitRight;
    private float screenWidth;
    private float spriteSize;

    void Start()
    {
        // infos sur l'ennemi
        currentHealth = maxHealth;          // santé max par défaut
        currentState = EnemyState.Flying;   // "vol" par défaut
        moveSpeed = .5f;
        enemyBody = GetComponent<Rigidbody2D>();
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // limite gauche écran et largeur totale
        Camera mainCamera = Camera.main;
        screenLimitLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        screenLimitRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        screenWidth = screenLimitRight - screenLimitLeft;

        // récupération du player
        player = GameObject.FindGameObjectWithTag("Player");

        // récupération de l'instance d'AudioManager
        audioInstance = AudioManager.instance;
    }

    void Update()
    {
        // switch ACTIONS/COMPORTEMENTS
        switch (currentState)
        {
            case EnemyState.Flying:
                Fly();
                break;

            default:
                break;
        }
    }

    public void TakeDamage(float damage) // Changed parameter type to float
    {
        // Réduire la santé par le montant de dégâts
        currentHealth -= damage;

        // Vérifier si l'ennemi est mort (santé ≤ 0)
        if (currentHealth <= 0)
        {
            DropHead();
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // collision avec le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<IDamageable>();
            if (player != null)
            {
                Debug.Log("Player hit");
                player.TakeDamage(20); // Player takes 10 damage

                // Play sound and destroy the UFO
                SoundOfDeath();
                DropHead();
                Destroy(gameObject); // Destroy the UFO after collision
            }
            else
            {
                Debug.LogError("Player is not IDamageable");
            }
        }
    }

    public void Shoot()
    {
        // le ufo ne shoot pas
    }

    public void Fly()
    {
        Vector3 currentPosition = transform.position;
        if (player != null)
        {
            Vector3 targetPosition = player.transform.position;
            transform.position = Vector3.Lerp(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
        }
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
    public int getCurrentHealth()
    {
        return 1;
    }
    public void Die()
    {
        // Notify the score system
        GetComponent<EnemyDeathNotifier>()?.NotifyDeath();

        // Destroy the UFO
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
