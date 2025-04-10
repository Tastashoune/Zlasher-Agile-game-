using UnityEngine;
using MyInterface;

public class EnemyUfo : MonoBehaviour, IEnemyInterface
{
    [Header("Health setting")]
    public int maxHealth = 50;          // Santé maximale de l'ennemi

    [Header("Player gameobject/transform")]
    public Transform player;

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
        //currentEnemy = EnemyType.Ufo;
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
    }
    void Update()
    {
        // switch ACTIONS/COMPORTEMENTS
        switch (currentState)
        {
            case EnemyState.Flying:
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = player.position;
                transform.position = Vector3.Lerp(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
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
                player.TakeDamage(10);
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

    }
    public void Die()
    {
        Destroy(gameObject);
    }
}