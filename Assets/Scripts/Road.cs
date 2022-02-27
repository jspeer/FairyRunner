using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Road : MonoBehaviour
{
    public GameObject parentObj;
    public GameObject roadPrefab;
    public Vector3 lastPos;
    public float offset = 0.7071068f;

    private int roadCount = 0;
    private ObjectPool<GameObject> roadObjPool;

    private void Awake()
    {
        // Instantiate our object pool for Crystal GFX
        this.roadObjPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(this.roadPrefab, parent: parentObj.transform),
            actionOnGet: roadObj => {},
            actionOnRelease: roadObj => {},
            actionOnDestroy: roadObj => Destroy(roadObj.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    private void Start()
    {
        
    }

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
        GameObject g = roadObjPool.Get();
        g.transform.position = spawnPos;
        g.transform.rotation = Quaternion.Euler(0, 45, 0);
        StartCoroutine(destroyRoadObjectWithTimer(g, 10f));

        // save last position
        lastPos = g.transform.position;

        roadCount++;

        if (roadCount % 5 == 0)
            g.transform.GetChild(0).gameObject.SetActive(true);
    }

    private IEnumerator destroyRoadObjectWithTimer(GameObject roadObj, float timer)
    {
        yield return new WaitForSeconds(timer);
        roadObjPool.Release(roadObj);
    }
}
