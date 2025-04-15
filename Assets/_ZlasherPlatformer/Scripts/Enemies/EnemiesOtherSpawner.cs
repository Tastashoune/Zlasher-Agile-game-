using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesOtherSpawner : MonoBehaviour
{
    public int nbMaxEnemies = 10;
    public GameObject enemyPoliceman;
    public GameObject enemySpearman;
    public GameObject enemyUfo;

    // Spawn delays for each enemy type
    public float policemanMinSpawnDelay = 3f;
    public float policemanMaxSpawnDelay = 5f;
    public float spearmanMinSpawnDelay = 1f;
    public float spearmanMaxSpawnDelay = 3f;
    public float ufoMinSpawnDelay = 5f;
    public float ufoMaxSpawnDelay = 7f;

    // Growth rates for spawn delays
    public float policemanDelayGrowthRate = -0.02f; // Policeman spawns faster over time
    public float spearmanDelayGrowthRate = 0.01f;  // Spearman spawns slower over time
    public float ufoDelayGrowthRate = -0.03f;      // UFO spawns faster over time

    // Minimum spawn delay cap to ensure enemies always spawn
    public float minGlobalSpawnDelay = 0.5f;

    private float screenLimitRight;
    private float groundLevelY = 5.5f; // Adjust this to match your ground level

    private float gameStartTime;
    private int spawnCounter = 0;

    void Start()
    {
        Camera mainCamera = Camera.main;
        screenLimitRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        gameStartTime = Time.time;

        StartCoroutine(EnemyPop());
    }

    public IEnumerator EnemyPop()
    {
        while (true)
        {
            GameObject nextEnemy = GetNextEnemy();

            // Spawn the enemy at ground level
            float spriteSize = nextEnemy.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            Vector3 posEnemy = new Vector3(screenLimitRight + spriteSize / 2, groundLevelY, 0f);

            Instantiate(nextEnemy, posEnemy, Quaternion.identity);

            // Calculate dynamic spawn delays based on elapsed time
            float elapsedTime = Time.time - gameStartTime;
            float minDelay = GetDynamicMinDelay(nextEnemy, elapsedTime);
            float maxDelay = GetDynamicMaxDelay(nextEnemy, elapsedTime);

            // Ensure minDelay does not drop below the global cap
            minDelay = Mathf.Max(minDelay, minGlobalSpawnDelay);

            float delay = Random.Range(minDelay, maxDelay);
            spawnCounter++;
            yield return new WaitForSeconds(delay);
        }
    }

    private float GetDynamicMinDelay(GameObject enemy, float elapsedTime)
    {
        if (enemy == enemyPoliceman)
        {
            return Mathf.Max(minGlobalSpawnDelay, policemanMinSpawnDelay + (policemanDelayGrowthRate * elapsedTime));
        }
        else if (enemy == enemySpearman)
        {
            return Mathf.Max(minGlobalSpawnDelay, spearmanMinSpawnDelay + (spearmanDelayGrowthRate * elapsedTime));
        }
        else if (enemy == enemyUfo)
        {
            return Mathf.Max(minGlobalSpawnDelay, ufoMinSpawnDelay + (ufoDelayGrowthRate * elapsedTime));
        }
        return minGlobalSpawnDelay; // Default fallback
    }

    private float GetDynamicMaxDelay(GameObject enemy, float elapsedTime)
    {
        if (enemy == enemyPoliceman)
        {
            return Mathf.Max(minGlobalSpawnDelay, policemanMaxSpawnDelay + (policemanDelayGrowthRate * elapsedTime));
        }
        else if (enemy == enemySpearman)
        {
            return Mathf.Max(minGlobalSpawnDelay, spearmanMaxSpawnDelay + (spearmanDelayGrowthRate * elapsedTime));
        }
        else if (enemy == enemyUfo)
        {
            return Mathf.Max(minGlobalSpawnDelay, ufoMaxSpawnDelay + (ufoDelayGrowthRate * elapsedTime));
        }
        return minGlobalSpawnDelay; // Default fallback
    }

    private GameObject GetNextEnemy()
    {
        // Simple round-robin selection for demonstration
        int enemyType = spawnCounter % 3;
        if (enemyType == 0) return enemyPoliceman;
        if (enemyType == 1) return enemySpearman;
        return enemyUfo;
    }
}
