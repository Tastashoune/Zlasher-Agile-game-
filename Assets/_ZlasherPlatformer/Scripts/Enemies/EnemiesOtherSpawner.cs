using System.Collections;
using UnityEngine;

public class EnemiesOtherSpawner : MonoBehaviour
{
    public int nbMaxEnemies=10;
    public GameObject enemyPoliceman;
    public GameObject enemySpearman;
    public GameObject enemyUfo;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;

    private float screenLimitRight;
    private float screenHeight;

    void Start()
    {
        Camera mainCamera = Camera.main;
        screenLimitRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        Vector3 bottomLeftWorld = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topLeftWorld = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0));
        screenHeight = topLeftWorld.y - bottomLeftWorld.y;

        StartCoroutine(EnemyPop());
    }
    void Update()
    {
        
    }
    public IEnumerator EnemyPop()
    {
        GameObject randomEnemy;

        for(int a=0; a<nbMaxEnemies; a++)
        {
            // tirage au sort aléatoire de l'ennemi parmi les prefab importés
            int randomIndex = Random.Range(1, 4);
            switch (randomIndex)
            {
                case 1:
                    randomEnemy = enemyPoliceman;
                    break;

                case 2:
                    randomEnemy = enemySpearman;
                    break;

                case 3:
                default:
                    randomEnemy = enemyUfo;
                    break;

            }
            // on récupère la sprite size pour placer l'ennemi en dehors de l'écran lors de sa chute
            float spriteSize = randomEnemy.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            Vector3 posEnemy = new Vector3(screenLimitRight + spriteSize / 2, screenHeight /2, 0f);

            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            Instantiate(randomEnemy, posEnemy, Quaternion.identity);
            yield return new WaitForSeconds(randomDelay);
        }
    }
}
