﻿using UnityEngine;
using MyInterface;

public class EnemyCitizen : MonoBehaviour, IEnemyInterface
{
    [Header("Health setting")]
    public int maxHealth = 50;          // Santé maximale de l'ennemi

    [Header("Self walk (false by default)")]
    public bool selfwalk = false;

    private int currentHealth;          // Santé actuelle (initialisée dans Start)
    private EnemyState currentState;
    private EnemyType currentEnemy;
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
        currentEnemy = EnemyType.Citizen;
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
    }
    void Update()
    {
        float currentPosX = transform.position.x; // enemyBody.position.x;

        // switch ACTIONS/COMPORTEMENTS
        switch (currentState)
        {
            case EnemyState.Walking:
                if (enemyBody != null && selfwalk)
                {
                    // Calculer la direction tant que l'ennemi est dans l'écran
                    if (currentPosX > screenLimitLeft + spriteSize)
                    {
                        Vector2 direction = new Vector2(moveSpeed, enemyBody.linearVelocity.y);
                        enemyBody.linearVelocity = direction;
                    }
                }
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

    public void Shoot()
    {
        // le citizen ne shoot pas
    }
    public void Fly()
    {
        // le citizen ne fly pas
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}