using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    // Public game references
    public Transform rayStart;
    public GameObject crystalEffect;

    // Private vars
    private Rigidbody rb;
    private bool walkingRight = true;
    private Animator anim;
    private GameManager gameManager;

    // Use this for initialization
    void Awake()
    {
        // Define the rigid body
        this.rb = GetComponent<Rigidbody>();
        // Define animator
        this.anim = GetComponent<Animator>();
        // Define the game manager
        this.gameManager = FindObjectOfType<GameManager>();
    }

    // This method is commonly used to control cameras
    private void FixedUpdate()
    {
        if (!this.gameManager.gameStarted) {
            return;
        } else {
            this.anim.SetTrigger("gameStarted");
        }

        // Use delta time to automatically move us forward at a fixed rate
        this.rb.transform.position = this.transform.position + this.transform.forward * 2 * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the direction of the player movement
        if (Input.GetKeyDown(KeyCode.Space)) {
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
        if (this.transform.position.y < -2) {
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

            // Generate effects
            GameObject effectObj = Instantiate(this.crystalEffect, this.rayStart.transform.position, Quaternion.identity);
            // Destroy related objects
            Destroy(effectObj, 2);
            Destroy(c.gameObject);
        }
    }
}