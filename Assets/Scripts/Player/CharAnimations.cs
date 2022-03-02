using UnityEngine;

public class CharAnimations : MonoBehaviour
{
    private GameManager gameManager;
    private CharPhysics charPhysics;
    private Animator anim;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        charPhysics = GetComponent<CharPhysics>();

        // Attach to the game manager game started event
        this.gameManager.gameStartedEvent.AddListener(this.GameStarted);

        // Attach to the physics event
        this.charPhysics.charPhysicsEvent.AddListener(this.ChangeState);

        anim = this.GetComponent<Animator>();
    }

    private void GameStarted(bool gameStarted)
    {
        if (gameStarted)
            this.anim.SetTrigger("gameStarted");
    }

    private void ChangeState(CharPhysicsState charPhysicsState)
    {
        switch (charPhysicsState) {
            case CharPhysicsState.idle:
                this.anim.SetTrigger("gameStarted");
                break;
            case CharPhysicsState.running:
                this.anim.SetTrigger("isRunning");
                break;
            case CharPhysicsState.falling:
                this.anim.SetTrigger("isFalling");
                break;
        }
    }
}
