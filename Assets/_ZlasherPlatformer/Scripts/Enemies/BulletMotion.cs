using UnityEngine;

public class BulletMotion : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D bulletBody;
    private float screenLimitLeft;

    void Start()
    {
        Camera mainCamera = Camera.main;

        // infos sur le projectile
        moveSpeed = -8.0f;
        bulletBody = GetComponent<Rigidbody2D>();
        screenLimitLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
    }

    void Update()
    {
        float currentPosX = transform.position.x;

        if (bulletBody != null)
        {
            // Calculer la direction tant que le projectile est dans l'écran
            if (currentPosX > screenLimitLeft)
            {
                Vector2 direction = new Vector2(moveSpeed, bulletBody.linearVelocity.y);
                bulletBody.linearVelocity = direction;
                //Debug.Log($"gauche écran : {screenLimitLeft} current pos : {currentPosX}");
            }
            else
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collision avec le player
        if(collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(10);
            }
            else
            {
                Debug.LogError("Player is not IDamageable");
            }

            // destruction de la balle (bullet)
            Destroy(gameObject);
        }
    }
}
