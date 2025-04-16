using System.Collections;
using UnityEngine;

public class EnemyZblobSpawner : MonoBehaviour
{
    public int nbMaxZblob = 1;
    public GameObject enemyZblob;
    public float minSpawnDelay = 0.5f; // Reduced for higher spawn rate
    public float maxSpawnDelay = 1.5f; // Reduced for higher spawn rate

    private float screenLimitRight;
    private float groundLevelY = -5.5f; // Adjust this to match your ground level
    private float gameStartTime;

    void Start()
    {
        Camera mainCamera = Camera.main;
        screenLimitRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        // Set ground level (adjust as needed)
        groundLevelY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.1f, 0)).y;

        gameStartTime = Time.time;

        StartCoroutine(EnemyPop());
    }

    public IEnumerator EnemyPop()
    {
        SpawnZblob();
        yield return new WaitForSeconds(GetDynamicDelay());
    }

    private void SpawnZblob()
    {
        // Spawn the enemy at ground level
        float spriteSize = enemyZblob.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        Vector3 posZblob = new Vector3(screenLimitRight + spriteSize / 2, groundLevelY, 0f);

        Instantiate(enemyZblob, posZblob, Quaternion.identity);
    }

    private float GetDynamicDelay()
    {
        float elapsedTime = Time.time - gameStartTime;

        // Adjust delay dynamically to ensure high spawn rate early and late
        if (elapsedTime < 30f) // Early game
        {
            return Random.Range(minSpawnDelay, maxSpawnDelay);
        }
        else if (elapsedTime > 120f) // Late game
        {
            return Random.Range(minSpawnDelay * 0.8f, maxSpawnDelay * 0.8f); // Faster spawn late game
        }
        else // Mid game
        {
            return Random.Range(minSpawnDelay * 1.2f, maxSpawnDelay * 1.2f); // Slightly slower spawn mid game
        }
    }
}
