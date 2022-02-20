using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public GameObject roadPrefab;
    public Vector3 lastPos;
    public float offset = 0.7071068f;

    private int roadCount = 0;

    public void StartBuilding()
    {
        InvokeRepeating("CreateNewRoadPart", 1.0f, 0.5f);
    }

    public void CreateNewRoadPart()
    {
        Debug.Log("Create new road part");

        // Give a 50/50 chance to spawn left or right
        Vector3 spawnPos = Vector3.zero;
        float chance = Random.Range(0, 100);
        if (chance < 50) {
            // spawn left
            spawnPos = new Vector3(lastPos.x + offset, lastPos.y, lastPos.z + offset);
        } else {
            // spawn right
            spawnPos = new Vector3(lastPos.x - offset, lastPos.y, lastPos.z + offset);
        }

        // create a new block
        GameObject g = Instantiate(roadPrefab, spawnPos, Quaternion.Euler(0, 45, 0));
        Destroy(g, 10.0f);  // After 10 seconds, we can destroy the block

        // save last position
        lastPos = g.transform.position;

        roadCount++;

        if (roadCount % 5 == 0)
            g.transform.GetChild(0).gameObject.SetActive(true);
    }
}
