using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score;
    public int Score { get { return score; } }
    public int HighScore { get { return PlayerPrefs.GetInt("Highscore"); } }

    public void IncreaseScore()
    {
        this.score++;

        if (this.score > HighScore) {
            PlayerPrefs.SetInt("Highscore", this.score);
        }
    }

    public void ResetScore()
    {
        this.score = 0;
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("Highscore", 0);
    }
}
