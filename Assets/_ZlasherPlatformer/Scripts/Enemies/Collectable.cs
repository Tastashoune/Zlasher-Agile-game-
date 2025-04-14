using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private float floorY = -10f;
    private AudioManager audioInstance;

    private void Start()
    {
        // r�cup�ration de l'instance d'AudioManager
        audioInstance = AudioManager.instance;
    }
    private void Update()
    {
        // si la t�te n'est pas collect�e, et tombe sous le sol, elle est d�truite
        //Debug.Log("tete = "+transform.position.y);
        if(transform.position.y < floorY)
            Destroy(gameObject);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // collision avec le player
        if (collision.gameObject.CompareTag("Player"))
        {
            // ajout de vie au player
            PlayerMovementScript playerS = collision.gameObject.GetComponent<PlayerMovementScript>();
            if (playerS != null)
            {
                // jouer le son de collecte de bonus
                if (audioInstance != null)
                {
                    Debug.Log("audio OK");
                    audioInstance.audioSource.clip = audioInstance.playlist[(int)AudioManager.Sounds.BonusHead];
                    audioInstance.audioSource.Play();
                }

                playerS.TakeHealth(10);
            }

            // destruction de la t�te
            Destroy(gameObject);
        }
    }
}