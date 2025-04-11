using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the UI Text element for the score
    private float score; // The player's score
    private bool isCounting; // Whether the score is currently counting

    void Start()
    {
        score = 0f; // Initialize the score to 0
        isCounting = true; // Start counting when the game begins
    }

    void Update()
    {
        if (isCounting)
        {
            // Increment the score based on time
            score += Time.deltaTime;
            UpdateScoreUI();
        }
    }

    public void StopScore()
    {
        isCounting = false; // Stop counting the score
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // Update the score text on the UI
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
        }
    }
}
