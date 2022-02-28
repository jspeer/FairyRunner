using UnityEngine;

public class RoadObject : MonoBehaviour
{
    private GameManager gameManager;
    private PoolManager poolManager;

    private void Awake()
    {
        this.gameManager = FindObjectOfType<GameManager>();
        this.poolManager = this.gameManager.GetComponent<PoolManager>();
    }

    private void Update()
    {
        Vector2 screenPosition = gameManager.MainCamera.WorldToScreenPoint(transform.position);
        if (screenPosition.y <= gameManager.OffScreenYPos) {
            this.poolManager.roadObjPool.Release(this.gameObject);
        }
    }
}
