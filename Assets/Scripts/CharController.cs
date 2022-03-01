using System.Collections;
using UnityEngine;

public class CharController : MonoBehaviour
{
    // serialized game references
    [SerializeField] private Transform rayStart;

    // Private vars
    private Rigidbody rb;
    private bool walkingRight = true;
    private Animator anim;
    private GameManager gameManager;
    private PoolManager poolManager;
    private ScoreManager scoreManager;
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
        this.anim = GetComponent<Animator>();
        // Define the game manager
        this.gameManager = FindObjectOfType<GameManager>();
        // Get the pool manager
        this.poolManager = FindObjectOfType<PoolManager>();
        // Get the score manager
        this.scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void FixedUpdate()
    {
        if (!this.gameManager.gameStarted) {
            this.currentRunSpeed = this.gameManager.InitialRunSpeed;
            return;
        } else {
            // One shot call to co-routine on physics update to increase the run speed
            StartCoroutine(this.IncreaseRunSpeed());
            this.anim.SetTrigger("gameStarted");
        }

        // Use delta time to automatically move us forward at a fixed rate
        this.rb.transform.position = this.transform.position + this.transform.forward * currentRunSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    private void Update()
    {
        // TODO: MAKE RAY CASTING AN EVENT INSTEAD OF PART OF THE UPDATE() FUNCTION
        // Using ray casting, if there is no physics object below us { ... }
        RaycastHit hit;
        if (!Physics.Raycast(this.rayStart.position, -this.transform.up, out hit, Mathf.Infinity)) {
            // Set falling animation to be true
            this.anim.SetTrigger("isFalling");
        } else {
            // Disable the falling animation
            this.anim.SetTrigger("isRunning");
        }

        // If we're falling, restart the game
        if (this.transform.position.y < this.gameManager.DeathHeight) {
            this.gameManager.EndGame();
        }
    }

    // Switch the direction of the character
    public void Switch() // switched to public so input handler could access this method
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
        if (c.tag == "Crystal") {
            // Increase the score
            this.scoreManager.IncreaseScore();

            // Instantiate GFX from pool
            GameObject effectObj = this.poolManager.xtalGfxPool.Get();
            effectObj.transform.position = this.rayStart.transform.position;
            effectObj.transform.rotation = Quaternion.identity;
            // After effect is done, release the pool object
            StartCoroutine(this.destroyXtalGfxWithTimer(effectObj, 2)); // hard coded timer value for destroying crystal gfx, this is fine because the duration of the graphic is 5 seconds, but the visible part is only about 2 seconds
            // Hide the crystal object now
            c.gameObject.SetActive(false);
        }
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
