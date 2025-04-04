using UnityEngine;

public class EnemyCombatSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 50;      // Santé maximale de l'ennemi

    private int currentHealth;       // Santé actuelle (initialisée dans Start)
    private EnemyState currentState;
    private Rigidbody2D enemyBody;
    private float moveSpeed;
    private Vector3 screenLimit;
    private float spriteSize;

    private enum EnemyType
    {
        Citizen,        // 0
        Policeman,      // 1
        Spearman,       // 2
        Ufo             // 3
    }
    private enum EnemyState
    {
        Walking,        // 0
        Attacking       // 1
    }
    private enum EnemyAttack
    {
        NoAttack,       // 0
        MeleeAttack,    // 1
        RangedAttack    // 2
    }

    void Start()
    {
        currentHealth = maxHealth;          // santé max par défaut
        currentState = EnemyState.Walking;  // "marche" par défaut

        // infos sur l'ennemi
        moveSpeed = -2.0f;
        enemyBody = GetComponent<Rigidbody2D>();
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // limite gauche écran
        screenLimit = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Walking:
                if (enemyBody != null)
                {
                    // Calculer la direction tant que l'ennemi est dans l'écran
                    if(enemyBody.position.x > (screenLimit.x + spriteSize))
                    {
                        Vector2 direction = new Vector2(moveSpeed, enemyBody.linearVelocity.y);
                        enemyBody.linearVelocity = direction;
                    }
                }
                break;

            case EnemyState.Attacking:
                break;

            default:
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        // Réduire la santé par le montant de dégâts
        currentHealth -= damage;

        // Déclencher l'événement de changement de santé
        //onHealthChanged?.Invoke(currentHealth, maxHealth);

        // Vérifier si l'ennemi est mort (santé ≤ 0)
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {

    }
}
