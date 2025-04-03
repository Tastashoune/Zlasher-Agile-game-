using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject floor;

    [SerializeField]
    private float spawnRate = 1f;

    private float timer = 0;

    private void Update()
    {
        if(timer<spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
        SpawnFloor();
        timer = 0;
        }
    }
        
    void SpawnFloor()
    {
        Instantiate(floor, transform.position, Quaternion.identity);
    }


}
