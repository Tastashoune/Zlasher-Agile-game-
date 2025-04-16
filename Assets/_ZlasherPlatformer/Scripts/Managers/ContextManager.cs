using UnityEngine;

public class ContextManager : MonoBehaviour
{
    public static ContextManager Instance { get; private set; }
    public DialogueManager dialogueManager;
    public GameManager gameManager;
    public AudioManager audioManager;
    //public MusicManager musicManager;

    private void Awake()
    {
        if(Instance != null && Instance !=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


    }



}
