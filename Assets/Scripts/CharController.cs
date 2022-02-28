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
    }

    // This method is commonly used to control cameras
    private void FixedUpdate()
    {
        if (!this.gameManager.gameStarted) {
            this.currentRunSpeed = this.gameManager.InitialRunSpeed;
            return;
        } else {
            StartCoroutine(this.IncreaseRunSpeed());
            this.anim.SetTrigger("gameStarted");
        }

        // Use delta time to automatically move us forward at a fixed rate
        this.rb.transform.position = this.transform.position + this.transform.forward * currentRunSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the direction of the player movement
        if (Input.GetKeyDown(KeyCode.Space) ^ Input.GetMouseButtonDown(0)) {
            this.Switch();
        }

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
    private void Switch()
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
            this.gameManager.IncreaseScore();

            // Instantiate GFX from pool
            GameObject effectObj = this.poolManager.xtalGfxPool.Get();
            effectObj.transform.position = this.rayStart.transform.position;
            effectObj.transform.rotation = Quaternion.identity;
            // After effect is done, release the pool object
            StartCoroutine(this.destroyXtalGfxWithTimer(effectObj, 2));
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
