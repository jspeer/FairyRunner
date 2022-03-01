using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    private PlayerInput playerInput;

    private void Awake()
    {
        this.playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        // This is a main timer loop to increase the game speed and difficulty
        StartCoroutine(IncreaseGameSpeedByAmountAndTimer(this.gameSpeedIncreaseAmount, this.gameSpeedIncreaseTime));
    }

    // This is a main timer loop to increase the game speed and difficulty
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
        switch (this.gameStarted) {
            case true:
                playerInput.SwitchCurrentActionMap("Player");
                return;
            case false:
                playerInput.SwitchCurrentActionMap("UI");
                return;
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
}
