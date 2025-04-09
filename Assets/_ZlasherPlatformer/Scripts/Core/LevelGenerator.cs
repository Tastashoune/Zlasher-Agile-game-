using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private FloorSpawner floorSpawner;

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private MoveObjects moveObjects;

    //[SerializeField]
    //private GameObject objectPooler;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;
    [SerializeField]
    private float topSpeed;



    private void Start()
    {
        if(floorPrefab != null)
        {
            moveObjects = floorPrefab.GetComponent<MoveObjects>();
        }
        AccelerateFloor();

    }
    private void Update()
    {
        AccelerateFloor();
    }


    private void AccelerateFloor()
    {
       if(player.position.x < pointA.position.x)
        {
            moveObjects.moveSpeed = 4;
        }
       else if(player.position.x >= pointB.position.x)
        {
            moveObjects.moveSpeed = topSpeed;
        }
       else
        {
            float playerLerp = Mathf.InverseLerp(pointA.position.x, pointB.position.x, player.position.x);
            moveObjects.moveSpeed = Mathf.Lerp(4,topSpeed,playerLerp);
        }


    }
}
