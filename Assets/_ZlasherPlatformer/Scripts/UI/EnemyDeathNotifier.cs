using System;
using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    public static event Action OnEnemyKilled; // Global event for enemy death

    public void NotifyDeath()
    {
        OnEnemyKilled?.Invoke(); // Trigger the event
        //Destroy(gameObject); // Destroy the enemy GameObject
    }
}
