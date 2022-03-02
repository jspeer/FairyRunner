using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[System.Serializable]
public class PlayerClicked : UnityEvent { }

public class InputHandler : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerInput playerInput;
    private CharController charController;
    public PlayerClicked playerClickedEvent;

    private void Awake()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

        gameManager = FindObjectOfType<GameManager>();
        playerInput = gameManager.GetComponent<PlayerInput>();
        charController = FindObjectOfType<CharController>();
        gameManager.gameStartedEvent.AddListener(this.GameStarted);

        if (playerClickedEvent == null)
            playerClickedEvent = new PlayerClicked();
    }

    private void GameStarted(bool gameStartedBool)
    {
        switch (gameStartedBool) {
            case true:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                this.playerInput.SwitchCurrentActionMap("Player");
                break;
            case false:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                this.playerInput.SwitchCurrentActionMap("UI");
                break;
        }
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        this.gameManager.StartGame();
    }

    public void PauseGameEvent(InputAction.CallbackContext context)
    {
        this.PauseGame();
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        this.playerInput.SwitchCurrentActionMap("PausedGame");

        this.gameManager.gameState = GameState.Paused;
        this.gameManager.gameStateEvent.Invoke(GameState.Paused);
    }

    // Input handler event triggered
    public void ResumeGameEvent(InputAction.CallbackContext context)
    {
        this.GameStarted(true);
        this.gameManager.gameState = GameState.Playing;
        this.gameManager.gameStateEvent.Invoke(this.gameManager.gameState);
    }

    public void HideAllMenus(InputAction.CallbackContext context)
    {
        
    }

    public void PlayerClicked(InputAction.CallbackContext context)
    {
        this.playerClickedEvent.Invoke();
    }
}
