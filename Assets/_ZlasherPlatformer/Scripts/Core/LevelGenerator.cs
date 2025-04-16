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
    private float topSpeed = 20f;
    [SerializeField]
    private float minSpeed = 4f;

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

    private float timeElapsed = 0f;
    [Header("Speed over time settings")]
    [SerializeField]
    private float timeInterval = 7f;
    [SerializeField]
    private float speedIncrement = 2f;
    [SerializeField]
    private float maxSpeed = 100f;

    private void Awake()
    {
        lastFloorPosition = transform.position;
        int initQt = 3;
        for(int j = 0; j < initQt; j++)
        {
            for (int i = 0; i < floor.Length; i++)
            {
                MoveObjects floorPool = Instantiate(floor[i], transform).GetComponent<MoveObjects>();
                floorPool.gameObject.SetActive(false);
                MoveObjectsPool.Add(floorPool);
            }
        }
        // TODO: Expand on logic more if we have more time


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
        // Level Generator speed over time
        // Augmenter les vitesses toutes les 7 secondes
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= timeInterval)
        {
            timeElapsed = 0f; // Réinitialiser le compteur
            // utilization of the maxSpeed
            if(topSpeed < maxSpeed)
            {
                minSpeed += speedIncrement;
                topSpeed += speedIncrement;
                Debug.Log($"topSpeed = {topSpeed}");
            }
        }
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
                moveObject.moveSpeed = minSpeed;
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
            float platformMoveSpeed = GetPlatformMoveSpeed();
            foreach (MoveObjects moveObject in spawnedFloors)
            {
                moveObject.moveSpeed = platformMoveSpeed;
            }
           
       }

        //Debug.Log(spawnedFloors[0].moveSpeed);
    }
    public float GetPlatformMoveSpeed()
    {
        float playerLerp = Mathf.InverseLerp(pointA.position.x, pointB.position.x, player.position.x);
        return Mathf.Lerp(minSpeed, topSpeed, playerLerp);        
    }
}
