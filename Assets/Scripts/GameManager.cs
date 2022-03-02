using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState {
    MainMenu,
    Playing,
    Paused,
    End
}

[System.Serializable]
public class GameStateEvent : UnityEvent<GameState> { }

[System.Serializable]
public class GameStarted : UnityEvent<bool> { }

[System.Serializable]
public class TimerTick : UnityEvent { }

public class GameManager : MonoBehaviour
{
    // serialized game references
    [Header("General")]
    [HideInInspector] public bool gameStarted;
    [SerializeField] private Camera mainCamera;
    public Camera MainCamera { get { return mainCamera; } }
    [SerializeField] private Transform cameraTarget;
    public Transform CameraTarget { get { return cameraTarget; } }

    [Header("Mechanics")]
    [SerializeField] private float gameSpeedStartValue = 1f;
    [SerializeField] private float gameSpeedIncreaseAmount = 0.01f;
    [SerializeField] private float gameSpeedIncreaseTime = 0.5f;

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
    [SerializeField] private List<Material> roadMaterials;
    public List<Material> RoadMaterials { get { return roadMaterials; } }

    [Header("Character")]
    [SerializeField] float initialRunSpeed = 2f;
    public float InitialRunSpeed { get { return initialRunSpeed; } }
    [SerializeField] float runSpeedFactor = 0.1f;
    public float RunSpeedFactor { get { return runSpeedFactor; } }
    [SerializeField] float deathHeight = -2f;
    public float DeathHeight { get { return deathHeight; } }

    // private vars
    private float currentGameSpeed = 1f;
    public float CurrentGameSpeed { get { return currentGameSpeed; } }
    public GameStarted gameStartedEvent;
    public TimerTick timerTickEvent;
    public GameStateEvent gameStateEvent;
    public GameState gameState;

    private void Awake()
    {
        // Construct our event systems
        if (this.gameStartedEvent == null)
            this.gameStartedEvent = new GameStarted();

        if (this.timerTickEvent == null)
            this.timerTickEvent = new TimerTick();

        if (this.gameStateEvent == null)
            this.gameStateEvent = new GameStateEvent();

        this.gameState = GameState.MainMenu;
    }

    private void Update()
    {
        List<GameState> validTimeScaleStates = new List<GameState>{
            GameState.Paused,
            GameState.Playing
        };

        if (validTimeScaleStates.Contains(this.gameState)) {
            switch (this.gameState) {
                case GameState.Paused:
                    if (Time.timeScale != 0) Time.timeScale = 0;
                    break;
                case GameState.Playing:
                    if (Time.timeScale == 0) Time.timeScale = 1;
                    break;
            }
        }
    }

    private void Start()
    {
        // This is a main timer loop to increase the game speed and difficulty
        StartCoroutine(this.IncreaseGameSpeedByAmountAndTimer(this.gameSpeedIncreaseAmount, this.gameSpeedIncreaseTime));
    }

    // This is a main timer loop to increase the game speed and difficulty
    private IEnumerator IncreaseGameSpeedByAmountAndTimer(float amount, float timer)
    {
        while (true) {
            yield return new WaitForSeconds(timer);
            if (this.gameStarted) {
                this.currentGameSpeed += amount;
                this.timerTickEvent.Invoke();
            } else {
                this.currentGameSpeed = this.gameSpeedStartValue;
            }
        }
    }

    public void StartGame()
    {
        this.gameStarted = true;
        this.gameState = GameState.Playing;
        this.gameStateEvent.Invoke(this.gameState);
        this.gameStartedEvent.Invoke(this.gameStarted);
    }

    public void EndGame()
    {
        this.gameStarted = false;
        this.gameState = GameState.End;
        this.gameStateEvent.Invoke(this.gameState);
        this.gameStartedEvent.Invoke(this.gameStarted);
        SceneManager.LoadScene(0);
    }
}
