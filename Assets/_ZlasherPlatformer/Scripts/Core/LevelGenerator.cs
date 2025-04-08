using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private FloorSpawner floorSpawner;

    [SerializeField]
    private GameObject moveObjects;

    [SerializeField]
    private GameObject EnemySpawner;

    [SerializeField]
    private GameObject objectPooler;

    [SerializeField]
    private Transform player;

    //private void Start()
    //{
    //    floorSpawner.SpawnFloor();
    //}


}
