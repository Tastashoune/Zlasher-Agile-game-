using System.Collections;
using UnityEngine;

public class EnemyCitizenSpawner : MonoBehaviour
{
    public int nbMaxCitizen = 10;
    public GameObject enemyCitizen;
    public float minSpawnDelay = 0.5f; // Reduced for higher spawn rate
    public float maxSpawnDelay = 1.5f; // Reduced for higher spawn rate

    private float screenLimitRight;
    private float groundLevelY = -5.5f; // Adjust this to match your ground level
    private float gameStartTime;

    private int spawnCounter = 0;

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
        while (true)
        {
            // Spawn 3 EnemyCitizen for every other enemy type
            for (int i = 0; i < 3; i++)
            {
                SpawnCitizen();
                yield return new WaitForSeconds(GetDynamicDelay());
            }

            // Simulate delay for other enemy types (optional)
            yield return new WaitForSeconds(GetDynamicDelay());
        }
    }

    private void SpawnCitizen()
    {
        // Spawn the enemy at ground level
        float spriteSize = enemyCitizen.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        Vector3 posCitizen = new Vector3(screenLimitRight + spriteSize / 2, groundLevelY, 0f);

        Instantiate(enemyCitizen, posCitizen, Quaternion.identity);
        spawnCounter++;
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
