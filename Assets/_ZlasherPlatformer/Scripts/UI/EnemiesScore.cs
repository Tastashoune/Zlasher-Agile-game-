using UnityEngine;
using TMPro;

public class EnemyKillScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the UI Text element for the score
    private int score; // The player's score

    void Start()
    {
        score = 0; // Initialize the score to 0
        UpdateScoreUI();
    }

    private void OnEnable()
    {
        EnemyDeathNotifier.OnEnemyKilled += IncrementScore;
    }

    private void OnDisable()
    {
        EnemyDeathNotifier.OnEnemyKilled -= IncrementScore;
    }

    public void IncrementScore()
    {
        score++; // Increment the score by 1
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // Update the score text on the UI
            scoreText.text = "Enemies Killed: " + score.ToString();
        }
    }
}
