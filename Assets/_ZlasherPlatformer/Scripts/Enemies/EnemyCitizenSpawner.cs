using System.Collections;
using UnityEngine;

public class EnemyCitizenSpawner : MonoBehaviour
{
    public int nbMaxCitizen=10;
    public GameObject enemyCitizen;
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
    public IEnumerator EnemyPop()
    {
        // on récupère la sprite size pour placer l'ennemi en dehors de l'écran lors de sa chute
        float spriteSize = enemyCitizen.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        Vector3 posCitizen = new Vector3(screenLimitRight+spriteSize/2, screenHeight/2, 0f);

        for(int a=0; a<nbMaxCitizen; a++)
        {
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            //GameObject newEnemy = 
            // on set l'objet instancié au parent "EnemyCitizenSpawner"
            //Instantiate(enemyCitizen, posCitizen, Quaternion.identity, transform);
            Instantiate(enemyCitizen, posCitizen, Quaternion.identity);
            //Debug.Log(newEnemy.transform.parent.name);
            //newEnemy.transform.SetParent(GameObject.FindGameObjectWithTag("Floor").transform);
            yield return new WaitForSeconds(randomDelay);
        }
    }
}
