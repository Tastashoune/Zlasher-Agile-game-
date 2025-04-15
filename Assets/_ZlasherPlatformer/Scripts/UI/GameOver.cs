using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
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
