using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int tileScoreValue = 1;
    [SerializeField] int tilePointSpread = 4;
    [SerializeField] int gemScoreValue = 10;
    private int tileCount;
    private int score;
    public int Score { get { return score; } }
    private int highScore;
    public int HighScore { get { return highScore; } }

    private void Awake()
    {
        this.score = 0;
        this.highScore = PlayerPrefs.GetInt("Highscore");
    }

    private void IncreaseScore(int increaseVal)
    {
        this.score += increaseVal;

        if (this.score > this.highScore) {
            this.SetHighScore(this.score);
        }
    }

    public void IncreaseGemCount()
    {
        this.IncreaseScore(this.gemScoreValue);
    }

    public void IncreaseTileCount()
    {
        this.tileCount++;
        if (this.tileCount % this.tilePointSpread == 0)
            this.IncreaseScore(this.tileScoreValue);
    }

    public void ResetScore()
    {
        this.score = 0;
    }

    private void SetHighScore(int score)
    {
        PlayerPrefs.SetInt("Highscore", score);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("Highscore", 0);
    }
}
