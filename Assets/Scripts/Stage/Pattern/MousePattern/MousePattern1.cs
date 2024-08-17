using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePattern1 : MonoBehaviour, IPattern
{
    #region Platform
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject left;
    [SerializeField] private PlatformEnemy platformPrefab;
    [SerializeField] private float platformSpeed = 5f;
    [SerializeField] private float platformInterval = 1f;
    private float platformTimer = 0f;
    private List<PlatformEnemy> platforms = new();
    #endregion

    #region Ray
    [SerializeField] private Ray rayPrefab;
    [SerializeField] private float rayIntervalMin = 2.5f;
    [SerializeField] private float rayIntervalMax = 3f;
    [SerializeField] private float rayXScale = 9f;
    private float rayInterval;
    private float rayTimer = 0f;
    private List<Ray> rays = new();
    #endregion

    public void Initialize(Player player)
    {
        platformTimer = Time.time;
        rayInterval = Random.Range(rayIntervalMin, rayIntervalMax);
        rayTimer = Time.time;
    }

    public void OnUpdate()
    {
        if(Time.time >= platformTimer + platformInterval)
        {
            platformTimer = Time.time;
            SpawnTwoPlatforms();
        }
        if(Time.time >= rayTimer + rayInterval)
        {
            rayTimer = Time.time;
            rayInterval = Random.Range(rayIntervalMin, rayIntervalMax);
            SpawnRay();
        }
    }

    private void SpawnTwoPlatforms()
    {
        PlatformEnemy rightPlatform = Managers.ObjectPool.GetObject(platformPrefab.gameObject).GetComponent<PlatformEnemy>();
        PlatformEnemy leftPlatform = Managers.ObjectPool.GetObject(platformPrefab.gameObject).GetComponent<PlatformEnemy>();
        rightPlatform.Move(right.transform.position, Vector2.down * platformSpeed);
        leftPlatform.Move(left.transform.position, Vector2.up * platformSpeed);
        platforms.Add(rightPlatform); platforms.Add(leftPlatform);
    }

    private void SpawnRay()
    {
        Ray ray = Managers.ObjectPool.GetObject(rayPrefab.gameObject).GetComponent<Ray>();
        rays.Add(ray);
        int random = Random.Range(0, 2);
        if(random == 0)
        {
            ray.transform.position = right.transform.position;
        }
        else
        {
            ray.transform.position = left.transform.position;
        }
        ray.ReadyRay(rayXScale);
    }

    public void Destroy()
    {
        foreach(PlatformEnemy platform in platforms)
        {
            platform.ReturnToPool();
        }
        foreach(Ray ray in rays)
        {
            ray.ReturnToPool();
        }
        platforms.Clear();
        rays.Clear();
    }
}
