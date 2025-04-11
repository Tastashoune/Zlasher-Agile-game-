using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    public int damage = 20; // Damage dealt by the SwordSlash

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage); // Deal damage to the enemy
            Destroy(gameObject); // Destroy the SwordSlash after hitting
        }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
