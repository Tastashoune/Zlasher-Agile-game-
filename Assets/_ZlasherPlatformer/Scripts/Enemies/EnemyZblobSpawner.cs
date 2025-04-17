using System.Collections;
using UnityEngine;

public class EnemyZblobSpawner : MonoBehaviour
{
    public int nbMaxZblob = 10; // Maximum number of Zblobs to spawn
    public GameObject enemyZblob;
    public float minSpawnDelay = 0.5f; // Minimum delay between spawns
    public float maxSpawnDelay = 1.5f; // Maximum delay between spawns

    private float screenLimitLeft;
    private float screenLimitRight;
    private float screenLimitTop;

    void Start()
    {
        Camera mainCamera = Camera.main;

        // Get screen limits
        screenLimitLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        screenLimitRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        screenLimitTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        StartCoroutine(SpawnZblobs());
    }

    private IEnumerator SpawnZblobs()
    {
        for (int i = 0; i < nbMaxZblob; i++)
        {
            SpawnZblob();

            // Wait for a random delay before spawning the next Zblob
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    private void SpawnZblob()
    {
        // Generate a random X position within the camera's horizontal limits
        float randomX = Random.Range(screenLimitLeft, screenLimitRight);

        // Spawn the Zblob at the top of the screen with the random X position
        Vector3 spawnPosition = new Vector3(randomX, screenLimitTop, 0f);
        Instantiate(enemyZblob, spawnPosition, Quaternion.identity);
    }
}
