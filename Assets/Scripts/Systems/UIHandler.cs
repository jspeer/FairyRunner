using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("Score Overlay")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    private ScoreManager scoreManager;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void FixedUpdate()
    {
        updateUIscore();
    }

    private void updateUIscore()
    {
        this.scoreText.text = $"Score: {scoreManager.Score.ToString()}";
        // Get high score
        this.highScoreText.text = $"Best: {scoreManager.HighScore.ToString()}";
    }
}
