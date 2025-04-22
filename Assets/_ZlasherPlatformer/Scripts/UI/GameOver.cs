using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private MusicManager audioInstance;

    private void Start()
    {
        // stop the background music
        audioInstance = MusicManager.instance;
        if (audioInstance != null)
        {
            audioInstance.audioSource.Stop();
        }
    }
    public void Restart()
    {
        
        SceneManager.LoadScene("MainScene");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
