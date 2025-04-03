using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject floor;

    [SerializeField]
    private float spawnRate = 1f;

    private float timer = 0;

    private float floorWidth = 20f;


    private void Start()
    {
        SpawnFloor();
    }
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
        Vector3 spawPosition = new Vector3(transform.position.x + floorWidth, transform.position.y, transform.position.y);
        Instantiate(floor, spawPosition, Quaternion.identity);
    }


}
