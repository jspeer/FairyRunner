using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;  // Target position
    private Vector3 offset;   // Offset to player

    void Awake()
    {
        // Define player offset
        this.offset = this.transform.position - this.target.position;
    }

    private void LateUpdate()
    {
        // Move the camera
        this.transform.position = this.target.position + this.offset;
    }
}
