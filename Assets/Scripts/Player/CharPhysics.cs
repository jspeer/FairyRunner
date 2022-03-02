using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharPhysicsState
{
    idle,
    running,
    falling
}

[System.Serializable]
public class CharPhysicsEvent : UnityEvent<CharPhysicsState> { }

public class CharPhysics : MonoBehaviour
{
    [SerializeField] private Transform physicsRayStart;
    [SerializeField] private CharPhysicsState gameplayState;
    public Transform PhysicsRayStart { get { return physicsRayStart; } }
    private CharPhysicsState charPhysicsState;
    public CharPhysicsState PhysicsState { get { return charPhysicsState; } }

    public CharPhysicsEvent charPhysicsEvent;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        // Attach to the game manager game started event
        this.gameManager.gameStartedEvent.AddListener(this.GameStarted);

        if (charPhysicsEvent == null)
            charPhysicsEvent = new CharPhysicsEvent();

        charPhysicsState = CharPhysicsState.idle;
    }

    private void GameStarted(bool gameStarted)
    {
        // The game has started, set the initial state to running
        if (gameStarted)
            charPhysicsState = gameplayState;
    }

    private void FixedUpdate()
    {
        // Grab previous state
        CharPhysicsState prevState = charPhysicsState;

        // Predefine what states can be used to check for falling
        List<CharPhysicsState> checkForFallingValidStates = new List<CharPhysicsState>{
            CharPhysicsState.falling,
            CharPhysicsState.running
        };

        // Only executing on valid state
        if (checkForFallingValidStates.Contains(charPhysicsState)) {
            charPhysicsState = CheckForFalling() ? CharPhysicsState.falling : CharPhysicsState.running;
        }

        // If the state changed, invoke a physics event
        if (charPhysicsState != prevState)
            charPhysicsEvent.Invoke(charPhysicsState);
    }

    private bool CheckForFalling()
    {
        RaycastHit hit;
        return !Physics.Raycast(this.physicsRayStart.position, -this.transform.up, out hit, Mathf.Infinity);
    }
}
