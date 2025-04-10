using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public float scrollSpeed = 1f; // Vitesse de défilement de la texture
    public GameObject backg1;
    private Renderer rend1;

    void Start()
    {
        rend1 = backg1.GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend1.material.mainTextureOffset = new Vector2(offset, 0f);
        Debug.Log("Texture Offset: " + rend1.material.mainTextureOffset);
    }
}
