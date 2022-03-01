using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerInput playerInput;
    private CharController charController;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerInput = gameManager.GetComponent<PlayerInput>();
        charController = FindObjectOfType<CharController>();
    }

    private void Update()
    {
        switch (gameManager.gameStarted) {
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
        gameManager.StartGame();
    }

    public void PlayerClicked(InputAction.CallbackContext context)
    {
        charController.Switch();
    }
}
