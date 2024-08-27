using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPattern2 : MonoBehaviour, IPattern
{
    [Header("Platform")]
    [SerializeField] private RunPlatform platformPrefab;
    [SerializeField] private RunPlatform initPlatformPrefab;
    [SerializeField] private Vector2 initPlatformPos;
    [SerializeField] private Vector2 platformPos;
    [SerializeField] private float platformYDistance;
    [SerializeField] private float platformInterval = 1f;
    private float platformTimer = 0f;
    private List<RunPlatform> platforms = new();

    [Header("Ray")]
    [SerializeField] private RayEnemy rayPrefab;
    [SerializeField] private float rayInterval = 1.5f;
    [SerializeField] private float rayScale = 3f;
    [SerializeField] private float rayXPos = 12f;
    [SerializeField] private float rayYFloor1 = -3f;
    private float rayTimer = 0f;
    private List<RayEnemy> rays = new();

    public void Initialize(Player player)
    {
        transform.position = Vector2.zero;
        SpawnThreePlatforms(initPlatformPrefab, initPlatformPos);
        for (int i = 2; i < 5; i++)
        {
            SpawnThreePlatforms(platformPrefab, initPlatformPos + 5 * i * Vector2.right);
        }
        platformTimer = rayTimer = Time.time;
    }

    public void OnUpdate()
    {
        if(Time.time >= platformTimer + platformInterval)
        {
            platformTimer = Time.time;
            SpawnThreePlatforms(platformPrefab, platformPos);
        }
        if (Time.time >= rayTimer + rayInterval)
        {
            rayTimer = Time.time;
            SpawnRay();
        }
    }

    public void Destroy()
    {
        foreach (var platform in platforms)
        {
            platform.ReturnToPool();
        }
        foreach (var ray in rays)
        {
            ray.ReturnToPool();
        }
        platforms.Clear(); rays.Clear();
    }
    
    private void SpawnThreePlatforms(RunPlatform prefab, Vector2 pos)
    {
        for(int i = 0; i < 3; i++)
        {
            RunPlatform runPlatform = Managers.ObjectPool.GetObject(prefab.gameObject).GetComponent<RunPlatform>();
            runPlatform.Move(new Vector2(pos.x, pos.y + platformYDistance * i), Vector2.left);
            platforms.Add(runPlatform);
        }
    }

    private void SpawnRay()
    {
        RayEnemy ray = Managers.ObjectPool.GetObject(rayPrefab.gameObject).GetComponent<RayEnemy>();

        int floor = Random.Range(1, 4);
        float rayYPos = rayYFloor1 + (floor - 1) * platformYDistance;
        ray.transform.position = new Vector2(rayXPos, rayYPos);
        ray.ReadyRay(rayScale);
        rays.Add(ray);
    }
}
