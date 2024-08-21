using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPattern0 : MonoBehaviour, IPattern
{
    [Header("Platform Prefab")]
    [SerializeField] private RunPlatform platformPrefab;
    [SerializeField] private RunPlatform platformEnemyPrefab;
    [SerializeField] private Vector2 initPlatformPos;
    [Header("Platform Spawn Interval")]
    [SerializeField] private float platformIntervalMax = 1f;
    [SerializeField] private float platformIntervalMin = 0.5f;
    private float platformInterval;
    [Header("EnemyPlatform Spawn Interval")]
    [SerializeField] private float enemyPlatformIntervalMax = 1.5f;
    [SerializeField] private float enemyPlatformIntervalMin = 2.5f;
    private float enemyPlatformInterval;
    [Header("Platform Spawn Position")]
    [SerializeField] private float platformXPos;
    [SerializeField] private float platformYMax;
    [SerializeField] private float platformYMin;

    private float platformTimer = 0f;
    private float enemyPlatformTimer = 0f;
    private List<RunPlatform> platforms = new();

    public void Initialize(Player player)
    {
        SpawnPlatform(initPlatformPos);
        SpawnRandomPlatform();

        platformTimer = Time.time;
        enemyPlatformTimer = Time.time;
        platformInterval = Random.Range(platformIntervalMin, platformIntervalMax);
        enemyPlatformInterval = Random.Range(enemyPlatformIntervalMin, enemyPlatformIntervalMax);
    }

    public void OnUpdate()
    {
        if(Time.time >= platformTimer + platformInterval)
        {
            platformTimer = Time.time;
            platformInterval = Random.Range(platformIntervalMin, platformIntervalMax);
            SpawnRandomPlatform();

            if (Time.time >= enemyPlatformTimer + enemyPlatformInterval)
            {
                enemyPlatformTimer = Time.time;
                enemyPlatformInterval = Random.Range(enemyPlatformIntervalMin, enemyPlatformIntervalMax);
                SpawnEnemyPlatform();
            }
        }
    }

    private void SpawnPlatform(Vector2 pos)
    {
        RunPlatform platform = Managers.ObjectPool.GetObject(platformPrefab.gameObject).GetComponent<RunPlatform>();
        platform.Move(pos, Vector2.left);
        platforms.Add(platform);
    }

    private void SpawnRandomPlatform()
    {
        float randomY = Random.Range(platformYMin, platformYMax);
        Vector2 pos = new(platformXPos, randomY);
        SpawnPlatform(pos);
    }

    private void SpawnEnemyPlatform()
    {
        float randomY = Random.Range(platformYMin, platformYMax);
        Vector2 pos = new(platformXPos + 9, randomY);
        RunPlatform platform = Managers.ObjectPool.GetObject(platformEnemyPrefab.gameObject).GetComponent<RunPlatform>();
        platform.Move(pos, Vector2.left, true);
        platforms.Add(platform);
    }

    public void Destroy()
    {
        foreach(RunPlatform platform in platforms)
        {
            platform.ReturnToPool();
        }
        platforms.Clear();
    }

}
