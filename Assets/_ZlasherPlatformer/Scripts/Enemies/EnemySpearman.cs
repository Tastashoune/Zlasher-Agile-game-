using UnityEngine;
using MyInterface;
public class EnemySpearman : MonoBehaviour, IEnemyInterface, IDamageable
{
    [Header("Health setting")]
    public int maxHealth = 50;          // Santé maximale de l'ennemi

    [Header("Self walk (false by default)")]
    public bool selfwalk = false;
    [Header("Collectable head")]
    public GameObject cHead;

    private AudioManager audioInstance;

    private int currentHealth;          // Santé actuelle (initialisée dans Start)
    private EnemyState currentState;
    //private EnemyType currentEnemy;
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
        //currentEnemy = EnemyType.Spearman;
        currentHealth = maxHealth;          // santé max par défaut
        currentState = EnemyState.Walking;  // "marche" par défaut
        moveSpeed = -2.0f;
        enemyBody = GetComponent<Rigidbody2D>();
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // limite gauche écran et largeur totale
        Camera mainCamera = Camera.main;
        screenLimitLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        screenLimitRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        screenWidth = screenLimitRight - screenLimitLeft;

        // récupération de l'instance d'AudioManager
        audioInstance = AudioManager.instance;
    }
    void Update()
    {
        // switch ACTIONS/COMPORTEMENTS
        switch (currentState)
        {
            case EnemyState.Walking:
                float currentPosX = transform.position.x; // enemyBody.position.x;

                if (enemyBody != null && selfwalk)
                {
                    // Calculer la direction tant que l'ennemi est dans l'écran
                    if (currentPosX > screenLimitLeft + spriteSize)
                    {
                        Vector2 direction = new Vector2(moveSpeed, enemyBody.linearVelocity.y);
                        enemyBody.linearVelocity = direction;
                    }
                }

                // destroy/object pooling si l'ennemi dépasse la gauche de l'écran
                if (currentPosX < screenLimitLeft)
                    Die();

                break;

            default:
            break;
        }
    }
    public void TakeDamage(int damage)
    {
        // Réduire la santé par le montant de dégâts
        currentHealth -= damage;

        // Vérifier si l'ennemi est mort (santé ≤ 0)
        if (currentHealth <= 0)
        {
            SoundOfDeath();
            DropHead();
            Die();
        }
    }

    public void Shoot()
    {
        //yield return new WaitForSeconds(2f);
    }
    public void Fly()
    {
        // le piquier ne vole/fly pas
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

        // attachement au sol dès qu'il le touche
        if (collision.gameObject.CompareTag("Floor"))
        {
            transform.SetParent(collision.gameObject.transform);
        }

        // collision avec le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<IDamageable>();
            if (player != null)
            {
                Debug.Log("Player hit");
                player.TakeDamage(10);
            }
            else
            {
                Debug.LogError("Player is not IDamageable");
            }
        }
    }
}