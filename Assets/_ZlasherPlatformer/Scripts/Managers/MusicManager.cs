using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] playlist;
    public AudioSource audioSource;
    public static MusicManager instance;

    public enum Sounds
    {
        ZombieStorm       // 0
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y'a plus d'une instance de MusicManager");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        // starting background music zombie storm
        if (instance != null)
        {
            Debug.Log("audio OK");
            audioSource.clip = playlist[(int)Sounds.ZombieStorm];
            audioSource.Play();
        }
    }
}