using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public AudioClip[] playlist;
    public AudioSource audioSource;
    public static AudioManager instance;

    public enum Sounds
    {
        LetsKill,       // 0
        EnemyKill,      // 1
        BonusHead,      // 2
        Death           // 3
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y'a plus d'une instance de AudioManager");
            return;
        }
        instance = this;
    }

    void Start()
    {
        // when we start the game, we play "let's kill"
        audioSource.clip = playlist[(int)Sounds.LetsKill];
        audioSource.Play();
    }
}
