using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{


    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;
    [SerializeField]
    private float topSpeed;

    [SerializeField]
    private GameObject[] floor;

    [SerializeField]
    private float floorWidth = 40f;

    [SerializeField]
    private Transform spawnPoint;

    private Vector3 lastFloorPosition;

    private List<MoveObjects> spawnedFloors = new List<MoveObjects>();
    private List<MoveObjects> MoveObjectsPool = new List<MoveObjects>();

    private int initialFloorCount = 3;
    private int maxFloorsCount = 3;


    private void Awake()
    {
        lastFloorPosition = transform.position;

        for (int i = 0; i < initialFloorCount; i++)
        {
            MoveObjects floorPool = Instantiate(floor[Random.Range(0, floor.Length)], transform).GetComponent<MoveObjects>();
            floorPool.gameObject.SetActive(false);
            MoveObjectsPool.Add(floorPool);
        }


    }

    private void Start()
    {
        for (int i = 0; i < initialFloorCount; i++)
        {
            SpawnFloor(true);
        }


    }

    private MoveObjects GetFromPool()
    {
        if(MoveObjectsPool.Count > 0)
        {
            MoveObjects current = MoveObjectsPool[Random.Range(0, MoveObjectsPool.Count)];
            MoveObjectsPool.Remove(current);
            current.gameObject.SetActive(true);
            return current;
        }

        return null;
    }

    private void ReturnToPool(MoveObjects moveObject)
    {
        moveObject.gameObject.SetActive(false);
        MoveObjectsPool.Add(moveObject);
    }


    private void Update()
    {
        //lastFloorPosition.x = transform.position.x + floorWidth;

        if (CanSpawnFloor())
        {
            SpawnFloor(false);
        }

        AccelerateFloor();
    }


    private bool CanSpawnFloor()
    {

        if (spawnedFloors[0].transform.position.x < -1.1 * floorWidth)
            return true;
        return false;
    }
    public void SpawnFloor(bool startSpawn)
    {
        MoveObjects newFloor;

        if (startSpawn)
        {
            newFloor = GetFromPool();
            newFloor.transform.position = lastFloorPosition;
            spawnedFloors.Add(newFloor);

            //lastFloorPosition = newFloor.transform.position + new Vector3(floorWidth*spawnedFloors.Count, 0f, 0f);
            lastFloorPosition = newFloor.transform.position + new Vector3(floorWidth, 0f, 0f);
            return;

        }

        if (spawnedFloors.Count >= maxFloorsCount)
        {
            MoveObjects oldestFloor = spawnedFloors[0];
            ReturnToPool(spawnedFloors[0]);
            spawnedFloors.RemoveAt(0);

        }

        newFloor = GetFromPool();
        newFloor.transform.position = spawnPoint.position;
        spawnedFloors.Add(newFloor);
    }


    private void AccelerateFloor()
    {
       if(player.position.x < pointA.position.x)
        {
            foreach(MoveObjects moveObject in spawnedFloors)
            {
                moveObject.moveSpeed = 4;
            }
        }
       else if(player.position.x >= pointB.position.x)
        {
            foreach (MoveObjects moveObject in spawnedFloors)
            {
                moveObject.moveSpeed = topSpeed;
            }
        }
       else
        {
            float playerLerp = Mathf.InverseLerp(pointA.position.x, pointB.position.x, player.position.x);
            foreach (MoveObjects moveObject in spawnedFloors)
            {
                moveObject.moveSpeed = Mathf.Lerp(4, topSpeed, playerLerp);
            }
           
        }


    }
}
