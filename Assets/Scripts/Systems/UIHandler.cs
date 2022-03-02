using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [Header("Score Overlay")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
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
        this.highScoreText.text = $"High Score: {scoreManager.HighScore.ToString()}";
    }
}
