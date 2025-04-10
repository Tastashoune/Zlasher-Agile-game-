using UnityEngine;
using MyInterface;
public class EnemyPoliceman : MonoBehaviour, IEnemyInterface, IDamageable
{
    [Header("Health setting")]
    public int maxHealth = 50;          // Santé maximale de l'ennemi

    [Header("Bullet prefab")]
    public GameObject bullet;           // prefab du projectile
    //private RaycastHit2D hit;

    private bool isFirstShoot = true;

    private float timeElapsed;
    [SerializeField]
    private float attackDelay;

    [Header("Self walk (false by default)")]
    public bool selfwalk = false;

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
        isFirstShoot = true;
        // infos sur l'ennemi
        //currentEnemy = EnemyType.Policeman;
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

        timeElapsed = 0f;
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

                // test du tag Player pour ne pas tirer sur le décor en cas de détection
                if (HasPlayerInFront())
                {
                    //Debug.Log(hit.collider);
                    currentState = EnemyState.Attacking;
                }
            break;

            case EnemyState.Attacking:

                if (timeElapsed > attackDelay || isFirstShoot)
                {
                    Shoot();
                    isFirstShoot = false;
                    timeElapsed = 0f;
                }
                timeElapsed += Time.deltaTime;
                //Debug.Log(timeElapsed);
                
                // test du tag Player pour ne pas tirer sur le décor en cas de détection
                if (!HasPlayerInFront())
                {
                    Debug.Log("LOL");
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
        // test et détection joueur
        // dessin du laser devant l'ennemi (pour tester le raycast)
        // Debug.DrawRay ...
        Vector2 origine = new Vector2(transform.position.x - spriteSize, transform.position.y);
        return Physics2D.Raycast(origine, Vector2.left, screenWidth, LayerMask.GetMask("Player"));
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

    public void Shoot()
    {
        Vector3 bulletPosition = new Vector3(transform.position.x - spriteSize, transform.position.y, transform.position.z);
        // prise en compte de la rotation du prefab projectile (mais ne sera pas nécessaire lorsque l'asset sera prêt)
        Instantiate(bullet, bulletPosition, bullet.transform.rotation);
    }
    public void Fly()
    {
        // le policeman ne fly pas
    }
    public void Die()
    {
        // à faire : pop de la tête collectable (bonus point de vie)

        // object pooling, au lieu du destroy on remet le sprite enemyCitizen à droite de l'écran
        transform.position = new Vector3(screenLimitRight, transform.position.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (selfwalk)
            return;

        // auto friction par rapport au sol
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
}