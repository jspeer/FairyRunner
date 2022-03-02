using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [Header("Score Overlay")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Canvas pauseOverlay;

    private GameManager gameManager;
    private ScoreManager scoreManager;

    private void Awake()
    {
        this.gameManager = FindObjectOfType<GameManager>();
        this.scoreManager = FindObjectOfType<ScoreManager>();

        // Attach to the game manager game state event
        this.gameManager.gameStateEvent.AddListener(this.UIGameState);
    }

    private void FixedUpdate()
    {
        this.updateUIscore();
    }

    private void updateUIscore()
    {
        this.scoreText.text = $"Score: {scoreManager.Score.ToString()}";
        // Get high score
        this.highScoreText.text = $"High Score: {scoreManager.HighScore.ToString()}";
    }

    private void UIGameState(GameState gameState)
    {
        switch (gameState) {
            case GameState.Paused:
                this.pauseOverlay.gameObject.SetActive(true);
                break;
            case GameState.Playing:
                this.pauseOverlay.gameObject.SetActive(false);
                break;
        }
    }

    private void ResumeGame()
    {

    }
}
