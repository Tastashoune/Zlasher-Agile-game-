using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed;

    //[SerializeField]
    //private float deadZone = 0;

    private LevelGenerator levelGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * (Time.deltaTime * moveSpeed);
        //if (transform.position.x < deadZone)
        //{
        //    Destroy(gameObject);
        //}
    }
}
