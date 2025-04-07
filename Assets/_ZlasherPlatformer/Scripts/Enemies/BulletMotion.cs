using UnityEngine;

public class BulletMotion : MonoBehaviour
{

    private Rigidbody2D bulletBody;
    private float moveSpeed;

    private float screenLimitLeft;

    void Start()
    {
        Camera mainCamera = Camera.main;

        // infos sur le projectile
        moveSpeed = -4.0f;
        bulletBody = GetComponent<Rigidbody2D>();
        screenLimitLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
    }

    void Update()
    {
        float currentPosX = transform.position.x;

        if (bulletBody != null)
        {
            // Calculer la direction tant que le projectile est dans l'�cran
            if (currentPosX > screenLimitLeft)
            {
                Vector2 direction = new Vector2(moveSpeed, bulletBody.linearVelocity.y);
                bulletBody.linearVelocity = direction;
                //Debug.Log($"gauche �cran : {screenLimitLeft} current pos : {currentPosX}");
            }
            else
                Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // collision avec le player

    }
}
