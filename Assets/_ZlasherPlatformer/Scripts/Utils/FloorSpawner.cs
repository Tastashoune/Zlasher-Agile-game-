using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class FloorSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] floor;

    [SerializeField]
    private float floorWidth = 47.5f;

    [SerializeField]
    private Transform spawnPoint;

    private Vector3 lastFloorPosition;

    private List<GameObject> spawnedFloors = new List<GameObject>();

    private int initialFloorCount = 4;
    private int maxFloorsCount = 4;

    private bool StartSpawn = true;



    private void Start()
    {
        lastFloorPosition = transform.position;
        for (int i =0; i<initialFloorCount; i++)
        {
            SpawnFloor();
        }
        StartSpawn = false;

    }


    private void Update()
    {
            //lastFloorPosition.x = transform.position.x+floorWidth;

            if (CanSpawnFloor())
        {
            SpawnFloor();
        }
       
    }


    private bool CanSpawnFloor()
    {

        if (spawnedFloors[0].transform.position.x < -1.1*floorWidth)
            return true;
        return false;
    }
    public void SpawnFloor()
    {
        if (spawnedFloors.Count >= maxFloorsCount)

        {
            GameObject oldestFloor = spawnedFloors[0];
            spawnedFloors.RemoveAt(0);
            Destroy(oldestFloor);
         
        }

        if (StartSpawn==true)
        {
            int randomIndex = Random.Range(0, floor.Length);
            GameObject newFloor = Instantiate(floor[randomIndex], lastFloorPosition, Quaternion.identity);
            spawnedFloors.Add(newFloor);


            //lastFloorPosition = newFloor.transform.position + new Vector3(floorWidth*spawnedFloors.Count, 0f, 0f);
            lastFloorPosition = newFloor.transform.position + new Vector3(floorWidth, 0f, 0f);
            
        }
        else

        {
            maxFloorsCount = 3;
            int randomIndex = Random.Range(0, floor.Length);
            GameObject newFloor = Instantiate(floor[randomIndex], spawnPoint.position, Quaternion.identity);
            spawnedFloors.Add(newFloor);


        }


    }

  //queue list


}
        