using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score;
    public Text scoreText;
    public Text highScoreText;

    public bool gameStarted;

    private void Awake()
    {
        this.scoreText.text = $"Score: {score.ToString()}";
        // Get high score
        this.highScoreText.text = $"Best: {GetHighScore().ToString()}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartGame();
        }
    }

    public void StartGame()
    {
        this.gameStarted = true;
        FindObjectOfType<Road>().StartBuilding();
    }

    public void EndGame()
    {
        this.gameStarted = false;
        SceneManager.LoadScene(0);
    }

    public void IncreaseScore()
    {
        this.score++;
        this.scoreText.text = $"Score: {score.ToString()}";

        if (this.score > GetHighScore()) {
            PlayerPrefs.SetInt("Highscore", this.score);
            this.highScoreText.text = $"Best: {GetHighScore().ToString()}";
        }
    }

    public int GetHighScore()
    {
        int i = PlayerPrefs.GetInt("Highscore");
        return i;
    }
}
