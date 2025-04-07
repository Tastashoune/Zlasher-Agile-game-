using UnityEngine;

public class EnemyCombatSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 50;          // Santé maximale de l'ennemi

    [Header("Bullet Settings")]
    public GameObject bullet;           // prefab du projectile
    public bool bullet1;
    public bool bullet2;

    private int currentHealth;          // Santé actuelle (initialisée dans Start)
    private EnemyState currentState;
    private EnemyType currentEnemy;
    private Rigidbody2D enemyBody;
    private float moveSpeed;

    // infos écran
    private float screenLimitLeft;
    private float screenLimitRight;
    private float screenWidth;
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
        Camera mainCamera = Camera.main;

        // infos sur l'ennemi
        currentHealth = maxHealth;          // santé max par défaut
        currentState = EnemyState.Walking;  // "marche" par défaut
        moveSpeed = -2.0f;
        enemyBody = GetComponent<Rigidbody2D>();
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        // limite gauche écran et largeur totale
        screenLimitLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        screenLimitRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        screenWidth = screenLimitRight - screenLimitLeft;

        // tirage aléatoire de l'ennemi pour le spawner : A PLACER DANS LE GAMEMANAGER qui choisira au hasard, PAS ICI
        int randomEnemy = Random.Range(0, System.Enum.GetValues(typeof(EnemyType)).Length);
        // cast en enum EnemyType car on a un entier en retour pour l'instant
        currentEnemy = (EnemyType)randomEnemy;
        // on force la valeur pour les tests only
        currentEnemy = EnemyType.Policeman;
    }

    void Update()
    {
        float currentPosX = transform.position.x; // enemyBody.position.x;

        // switch SCENARIOS d'ennemis
        // si l'ennemi tiré au sort est un EnemyType.Policeman, on le fait tirer, et s'arreter tous les quarts d'écran
        // A supprimer ensuite, car chaque prefab d'ennemi aura son propre script
        switch (currentEnemy)
        {
            case EnemyType.Policeman:
                // on fait tirer l'ennemi au 7/8 de l'écran
                if (currentPosX < screenLimitLeft + (screenWidth * 7 / 8) && !bullet1)
                {
                    currentState = EnemyState.Attacking;
                    bullet1 = true;
                }
                // on fait tirer l'ennemi au 3/4 de l'écran
                if (currentPosX < screenLimitLeft + (screenWidth * 3 / 4) && !bullet2)
                {
                    currentState = EnemyState.Attacking;
                    bullet2 = true;
                }
            break;

            default:
            break;
        }

        // switch ACTIONS/COMPORTEMENTS
        switch (currentState)
        {
            case EnemyState.Walking:
                if (enemyBody != null)
                {
                    // Calculer la direction tant que l'ennemi est dans l'écran
                    if(currentPosX > screenLimitLeft + spriteSize)
                    {
                        Vector2 direction = new Vector2(moveSpeed, enemyBody.linearVelocity.y);
                        enemyBody.linearVelocity = direction;
                    }
                }
            break;

            case EnemyState.Attacking:
                Debug.Log("on entre dans Attacking");
                Shoot();
                currentState = EnemyState.Walking;
            break;

            default:
            break;
        }
    }
    public void Shoot()
    {
        Vector3 bulletPosition = new Vector3(transform.position.x - spriteSize, transform.position.y, transform.position.z);
        // prise en compte de la rotation du prefab projectile (mais ne sera pas nécessaire lorsque l'asset sera prêt)
        Instantiate(bullet, bulletPosition, bullet.transform.rotation);
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
