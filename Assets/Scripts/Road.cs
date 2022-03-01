using System.Collections;
using UnityEngine;

public class Road : MonoBehaviour
{
    public int roadCount = 0;
    public int roadDestroyed = 0;
    private GameManager gameManager;
    private PoolManager poolManager;
    private CharController charController;

    private void Awake()
    {
        // Define the game manager
        this.gameManager = FindObjectOfType<GameManager>();

        // Get the pool manager
        this.poolManager = this.gameManager.GetComponent<PoolManager>();

        // Grab the character controller to obtain the run speed
        this.charController = FindObjectOfType<CharController>();
    }

    private void Start()
    {
        for (int i = 0; i < this.gameManager.MaxInitialBlocks; i++) {
            SpawnNewRoadPart();
        }
    }

    private void Update()
    {
        if (this.gameManager.gameStarted) {
            StartCoroutine(CreateNewRoadPart());
        }
    }

    private IEnumerator CreateNewRoadPart()
    {
        while (true) {
            // Clamp new speed to a minimum of 0
            float newSpeed = Mathf.Max(0, this.gameManager.InitialRoadBuildSpeed - (this.charController.CurrentRunSpeed * this.gameManager.RoadBuildSpeedFactor));
            // Spawn the part (cap the spawns to 100 -- hard limit for pooling)
            if (roadCount - roadDestroyed < this.gameManager.GetComponent<PoolManager>().MaxRoadPoolSize)
                SpawnNewRoadPart();
            // Go to sleep
            yield return new WaitForSeconds(newSpeed);
        }
    }

    private void SpawnNewRoadPart()
    {
        // get an object from the pool
        GameObject g = poolManager.roadObjPool.Get();
        // Set direction to 50/50 (left/right) from last position
        g.transform.position = new Vector3(this.gameManager.roadLastPos.x + (Random.Range(0, 2) % 2 == 0 ? this.gameManager.RoadOffset : -this.gameManager.RoadOffset), this.gameManager.roadLastPos.y, this.gameManager.roadLastPos.z + this.gameManager.RoadOffset);
        // Rotate 45 degrees
        g.transform.rotation = Quaternion.Euler(0, 45, 0);
        g.GetComponentInChildren<MeshRenderer>().material = this.gameManager.RoadMaterials[roadCount % this.gameManager.RoadMaterials.Count];

        // save last position
        this.gameManager.roadLastPos = g.transform.position;

        roadCount++;

        // Every 4 blocks, give a 50/50 chance to spawn a gem
        if (roadCount % 4 == 0 && Random.Range(0, 2) % 2 == 0)
            g.transform.GetChild(0).gameObject.SetActive(true);
    }
}
