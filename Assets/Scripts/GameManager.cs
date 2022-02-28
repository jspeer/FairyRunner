using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // serialized game references
    [Header("General")]
    [HideInInspector] public bool gameStarted;
    [SerializeField] private Camera mainCamera;
    public Camera MainCamera { get { return mainCamera; } }
    [SerializeField] private Transform cameraTarget;
    public Transform CameraTarget { get { return cameraTarget; } }

    [Header("Score Overlay")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    private int score;

    [Header("Mechanics")]
    [SerializeField] private float gameSpeedIncreaseAmount = 0.01f;
    [SerializeField] private float gameSpeedIncreaseTime = 0.5f;
    public float GameSpeedTimer { get { return gameSpeedIncreaseTime; } }

    [Header("Road")]
    [SerializeField] public Vector3 roadLastPos;
    [SerializeField] private float roadOffset;
    public float RoadOffset { get { return roadOffset; } }
    [SerializeField] private int maxInitialBlocks = 15;
    public int MaxInitialBlocks { get { return maxInitialBlocks; } }
    [SerializeField] private float offScreenYPos = -50f;
    public float OffScreenYPos { get { return offScreenYPos; } }
    [SerializeField] private float initialRoadBuildSpeed = 2f;
    public float InitialRoadBuildSpeed { get { return initialRoadBuildSpeed; } }
    [SerializeField] private float roadBuildSpeedFactor = .1f;
    public float RoadBuildSpeedFactor { get { return roadBuildSpeedFactor; } }
    
    [Header("Character")]
    [SerializeField] float initialRunSpeed = 2f;
    public float InitialRunSpeed { get { return initialRunSpeed; } }
    [SerializeField] float runSpeedFactor = 0.1f;
    public float RunSpeedFactor { get { return runSpeedFactor; } }

    // private vars
    private float currentGameSpeed = 1f;
    public float CurrentGameSpeed { get { return currentGameSpeed; } }

    private void Awake()
    {
        this.scoreText.text = $"Score: {score.ToString()}";
        // Get high score
        this.highScoreText.text = $"Best: {GetHighScore().ToString()}";
    }

    private void Start()
    {
        StartCoroutine(IncreaseGameSpeedByAmountAndTimer(this.gameSpeedIncreaseAmount, this.gameSpeedIncreaseTime));
    }

    private IEnumerator IncreaseGameSpeedByAmountAndTimer(float amount, float timer)
    {
        while (true) {
            yield return new WaitForSeconds(timer);
            if (this.gameStarted) {
                currentGameSpeed += amount;
            } else {
                currentGameSpeed = 1f;
            }
        }
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
