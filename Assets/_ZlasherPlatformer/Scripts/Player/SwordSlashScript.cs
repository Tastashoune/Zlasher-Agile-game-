using UnityEngine;

public class SwordSlashScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float minChargeTime = 0.5f;
    public float maxChargeTime = 2f;
    public float minProjectileSpeed = 5f;
    public float maxProjectileSpeed = 20f;
    public float cooldownTime = 1f;

    private float chargeTime;
    private bool isCharging;
    private bool canFire;
    private float cooldownTimer;
    private Animator animator;
    public int damage = 20; // Damage dealt by the SwordSlash

    private void Awake()
    {

        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chargeTime = 0f;
        isCharging = false;
        canFire = true;
        cooldownTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canFire && Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            StartCharging();
        }

        if (isCharging && Input.GetMouseButton(0)) // Holding the left mouse button
        {
            ChargeProjectile();
        }

        if (isCharging && Input.GetMouseButtonUp(0)) // Releasing the left mouse button
        {
            FireProjectile();
        }

        if (!canFire)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownTime)
            {
                canFire = true;
                cooldownTimer = 0f;
            }
        }
    }

    void StartCharging()
    {
        chargeTime = 0f;
        isCharging = true;
    }

    void ChargeProjectile()
    {
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
        }
    }

    void FireProjectile()
    {
        if (isCharging && chargeTime >= minChargeTime)
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
            isCharging = false;
            canFire = false;
            float chargeRatio = (chargeTime - minChargeTime) / (maxChargeTime - minChargeTime);
            float projectileSpeed = Mathf.Lerp(minProjectileSpeed, maxProjectileSpeed, chargeRatio);

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * projectileSpeed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage); // Deal damage to the enemy
            Destroy(gameObject); // Destroy the SwordSlash after hitting
        }
    }
}
