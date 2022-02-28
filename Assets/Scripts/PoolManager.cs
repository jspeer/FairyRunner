using System.ComponentModel;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour {
    [Header("Road Objects Pool")]
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject roadParentObject;
    [SerializeField] private int maxRoadPoolSize;
    public int MaxRoadPoolSize { get { return maxRoadPoolSize; } }
    public ObjectPool<GameObject> roadObjPool;

    [Header("Crystal Objects Pool")]
    [SerializeField] private GameObject crystalEffect;
    [SerializeField] private int maxXtalGfxPoolSize;
    public ObjectPool<GameObject> xtalGfxPool;

    // Private vars
    private GameManager gameManager;
    private Road road;
    private CharController charController;

    private void Awake()
    {
        // Load the game manager
        gameManager = FindObjectOfType<GameManager>();

        // Load the road objects
        road = FindObjectOfType<Road>();

        // Load the character controller
        charController = FindObjectOfType<CharController>();

        // Instantiate our object pool for Road Objects
        this.roadObjPool = new ObjectPool<GameObject>(
            createFunc: () => {
                GameObject roadObj = Instantiate(this.roadPrefab, parent: roadParentObject.transform);
                roadObj.AddComponent<RoadObject>();
                return roadObj;
            },
            actionOnGet: roadObj => roadObj.SetActive(true),
            actionOnRelease: roadObj => {
                roadObj.SetActive(false);
                road.roadDestroyed++;
            },
            actionOnDestroy: roadObj => Destroy(roadObj.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: maxRoadPoolSize
        );

        // Instantiate our object pool for Crystal GFX
        this.xtalGfxPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(this.crystalEffect),
            actionOnGet: effect => effect.GetComponent<ParticleSystem>().Play(),
            actionOnRelease: effect => {},
            actionOnDestroy: effect => Destroy(effect.gameObject),
            collectionCheck: true,
            defaultCapacity: 5,
            maxSize: this.maxXtalGfxPoolSize
        );
    }
}
