using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private AudioManager audioManager;
    public GameObject gameplayHUD;

    public void StartAfterIntro()
    {
        // backgrounds already OK/present in the scene
        // floors already OK
        // player already OK

        // déclenchement du défilement du sol (starting moving floors with "LevelGenerator")

        // starting moving Backgrounds

        // instanciate game UI (Canvas)

        // instanciate enemies spawners
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DialogueScene");
        audioManager = AudioManager.instance;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += CheckScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= CheckScene;
    }

    public void CheckScene(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "DialogueScene" )
        {
            if(audioManager == null)
            {
                audioManager = AudioManager.instance;
            }
            if (gameplayHUD != null)
                gameplayHUD.SetActive(false);
        }
        else
        {
            if (gameplayHUD != null)
                gameplayHUD.SetActive(true);
        }

        if (scene.name == "MainScene")
        {
            audioManager.audioSource.clip = audioManager.playlist[(int)AudioManager.Sounds.LetsKill];
            audioManager.audioSource.Play();
        }
        else
        {
            audioManager.audioSource.Stop();
            
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
