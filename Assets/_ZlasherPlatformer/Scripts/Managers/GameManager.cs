using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        
    }
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
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
