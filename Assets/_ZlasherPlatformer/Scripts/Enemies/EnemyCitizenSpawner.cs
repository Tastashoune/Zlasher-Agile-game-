using System.Collections;
using UnityEngine;

public class EnemyCitizenSpawner : MonoBehaviour
{
    public int nbMaxCitizen=10;
    public GameObject enemyCitizen;

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
        Vector3 posCitizen = new Vector3(screenLimitRight, screenHeight/2, 0f);

        for(int a=0; a<nbMaxCitizen; a++)
        {
            Instantiate(enemyCitizen, posCitizen, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }
}
