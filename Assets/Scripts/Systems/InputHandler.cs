using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private GameManager gameManager;
    private CharController charController;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        charController = FindObjectOfType<CharController>();
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
