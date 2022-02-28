using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private GameManager gameManager;
    private Vector3 offset;   // Offset to player

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        // Define player offset
        this.offset = this.transform.position - this.gameManager.CameraTarget.position;
    }

    private void LateUpdate()
    {
        // Move the camera
        this.transform.position = this.gameManager.CameraTarget.position + this.offset;
    }
}
