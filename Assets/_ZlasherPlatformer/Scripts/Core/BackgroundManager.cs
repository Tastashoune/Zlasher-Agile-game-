using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 0.05f; // Vitesse de défilement de base de la texture

    public GameObject backg1;
    public GameObject backg2;
    public GameObject backg3;
    public GameObject backg4;
    public GameObject backg5;

    private Renderer rend1;
    private Renderer rend2;
    private Renderer rend3;
    private Renderer rend4;
    private Renderer rend5;
    private LevelGenerator levelFloor;

    private float previousSpeed = 0f; // Stocker la dernière vitesse calculée

    void Start()
    {
        rend1 = backg1.GetComponent<Renderer>();
        rend2 = backg2.GetComponent<Renderer>();
        rend3 = backg3.GetComponent<Renderer>();
        rend4 = backg4.GetComponent<Renderer>();
        rend5 = backg5.GetComponent<Renderer>();

        // récupération de la vitesse du player pour se caler dessus
        GameObject levelGeneratorObject = GameObject.FindWithTag("LevelGenerator");
        levelFloor = levelGeneratorObject.GetComponent<LevelGenerator>();
    }

    void Update()
    {

        float platformMoveSpeed = levelFloor.GetPlatformMoveSpeed();
        platformMoveSpeed *= 0.01f;
        float baseSpeed = platformMoveSpeed * Time.deltaTime * scrollSpeed;

        float offset1 = baseSpeed * 0.2f;
        rend1.material.mainTextureOffset += new Vector2(offset1, 0f);

        float offset2 = baseSpeed * .5f;
        rend2.material.mainTextureOffset += new Vector2(offset2, 0f);

        float offset3 = baseSpeed;
        rend3.material.mainTextureOffset += new Vector2(offset3, 0f);

        float offset4 = baseSpeed * 1.5f;
        rend4.material.mainTextureOffset += new Vector2(offset4, 0f);

        float offset5 = baseSpeed * 2f;
        rend5.material.mainTextureOffset += new Vector2(offset5, 0f);
    }
}
