using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePattern1 : MonoBehaviour, IPattern
{
    #region Platform
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject left;
    [SerializeField] private MovingPlatform platformPrefab;
    [SerializeField] private float platformSpeed = 5f;
    [SerializeField] private float platformInterval = 1f;
    private float platformTimer = 0f;
    private List<MovingPlatform> platforms = new();
    #endregion

    #region Ray
    [SerializeField] private Ray rayPrefab;
    [SerializeField] private float rayIntervalMin = 2.5f;
    [SerializeField] private float rayIntervalMax = 3f;
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
        MovingPlatform rightPlatform = Managers.ObjectPool.GetObject(platformPrefab.gameObject).GetComponent<MovingPlatform>();
        MovingPlatform leftPlatform = Managers.ObjectPool.GetObject(platformPrefab.gameObject).GetComponent<MovingPlatform>();
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
        ray.ReadyRay();
    }

    public void Destroy()
    {
        foreach(MovingPlatform platform in platforms)
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
