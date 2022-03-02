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

    public void PauseGame(InputAction.CallbackContext context)
    {

    }

    public void HideAllMenus(InputAction.CallbackContext context)
    {
        
    }

    public void PlayerClicked(InputAction.CallbackContext context)
    {
        this.playerClickedEvent.Invoke();
    }
}
