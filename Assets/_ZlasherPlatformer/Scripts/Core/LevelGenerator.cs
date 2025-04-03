using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private FloorSpawner floorSpawner;

    [SerializeField]
    private GameObject moveObjects;

    [SerializeField]
    private GameObject EnemySpawner;

    private void Start()
    {
        floorSpawner.SpawnFloor();
    }


}
