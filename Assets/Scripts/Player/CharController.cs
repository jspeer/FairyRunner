using System.Collections;
using UnityEngine;

public class CharController : MonoBehaviour
{
    // Private vars
    private Rigidbody rb;
    private bool walkingRight = true;
    private GameManager gameManager;
    private PoolManager poolManager;
    private ScoreManager scoreManager;
    private CharPhysics charPhysics;
    private float currentRunSpeed;

    // Public methods
    public float CurrentRunSpeed { get { return currentRunSpeed; } }
    public float IncreasedRunSpeed { get { return currentRunSpeed - this.gameManager.InitialRunSpeed; } }

    // Use this for initialization
    private void Awake()
    {
        // Define the rigid body
        this.rb = GetComponent<Rigidbody>();
        // Define animator
        // Define the game manager
        this.gameManager = FindObjectOfType<GameManager>();
        // Get the pool manager
        this.poolManager = FindObjectOfType<PoolManager>();
        // Get the score manager
        this.scoreManager = FindObjectOfType<ScoreManager>();
        this.charPhysics = GetComponent<CharPhysics>();

        // Attach to the game manager game started event
        this.gameManager.gameStartedEvent.AddListener(this.GameStarted);
        // Attach to the game manager timer tick event
        this.gameManager.timerTickEvent.AddListener(this.TimerTick);

        // Attach to the player input player click event
        this.gameManager.GetComponent<InputHandler>().playerClickedEvent.AddListener(this.Switch);

        // Attach to the physics event
        this.charPhysics.charPhysicsEvent.AddListener(this.ApplyPhysics);
    }

    private void GameStarted(bool gameStarted)
    {
        this.currentRunSpeed = this.gameManager.InitialRunSpeed;
        if (gameStarted) {
            StartCoroutine(this.IncreaseRunSpeed());
        }
    }

    private void TimerTick()
    {
        StartCoroutine(this.IncreaseRunSpeed());
    }

    private void FixedUpdate()
    {
        // Use delta time to automatically move us forward at a fixed rate
        if (this.gameManager.gameStarted)
            this.rb.transform.position = this.transform.position + this.transform.forward * this.currentRunSpeed * Time.deltaTime;

        // If we're falling, call end the current game
        if (this.transform.position.y < this.gameManager.DeathHeight) {
            this.gameManager.EndGame();
        }
    }

    private void ApplyPhysics(CharPhysicsState charPhysicsState)
    {
        if (charPhysicsState == CharPhysicsState.falling) {
            // Set falling animation to be true
            // Disable the collider to prevent clipping
            this.GetComponent<MeshCollider>().enabled = false;
        } else {
            // Disable the falling animation
        }
    }

    // Switch the direction of the character
    private void Switch() // switched to public so input handler could access this method
    {
        if (!this.gameManager.gameStarted) {
            return;
        }

        this.walkingRight = !this.walkingRight;

        // Adjust character rotation offset by +/- 45 degrees
        if (this.walkingRight) {
            this.transform.rotation = Quaternion.Euler(0,  45, 0);
        } else {
            this.transform.rotation = Quaternion.Euler(0, -45, 0);
        }
    }

    // On collision with crystals { ... }
    private void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Crystal")
        {
            CollidedWithCrystal(c);
        }
    }

    private void CollidedWithCrystal(Collider c)
    {
        // Increase the score
        this.scoreManager.IncreaseGemCount();

        // Instantiate GFX from pool
        GameObject effectObj = this.poolManager.xtalGfxPool.Get();
        effectObj.transform.position = this.charPhysics.PhysicsRayStart.transform.position;
        effectObj.transform.rotation = Quaternion.identity;
        // After effect is done, release the pool object
        StartCoroutine(this.destroyXtalGfxWithTimer(effectObj, 2)); // hard coded timer value for destroying crystal gfx, this is fine because the duration of the graphic is 5 seconds, but the visible part is only about 2 seconds
                                                                    // Hide the crystal object now
        c.gameObject.SetActive(false);
    }

    private void OnCollisionExit(Collision other) {
        this.scoreManager.IncreaseTileCount();
    }

    private IEnumerator IncreaseRunSpeed()
    {
        yield return new WaitForSeconds(this.gameManager.CurrentGameSpeed);
        this.currentRunSpeed = this.gameManager.InitialRunSpeed + (this.gameManager.CurrentGameSpeed * this.gameManager.RunSpeedFactor);
    }

    private IEnumerator destroyXtalGfxWithTimer(GameObject effect, float timer)
    {
        yield return new WaitForSeconds(timer);
        this.poolManager.xtalGfxPool.Release(effect);
    }
}
