using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPattern1 : MonoBehaviour, IPattern
{
    [Header("CubeObstacle")]
    [SerializeField] private RunPlatform cubeObstaclePrefab;
    [SerializeField] private Vector2 cubeObstaclePos;
    [SerializeField] private Vector2 cubeObstacleDistance;
    [SerializeField] private float cubeObstacleInterval = 1f;
    private float cubeObstacleTimer = 0f;
    private List<RunPlatform> cubeObstacles = new();

    public void Initialize(Player player)
    {
        SpawnTwoCubeObstacles();
        cubeObstacleTimer = Time.time;
    }

    public void OnUpdate()
    {
        if (Time.time >= cubeObstacleTimer + cubeObstacleInterval)
        {
            cubeObstacleTimer = Time.time;
            SpawnTwoCubeObstacles();
        }
    }

    public void Destroy()
    {
        foreach(RunPlatform cubeObstacle in cubeObstacles)
        {
            cubeObstacle.ReturnToPool();
        }
    }

    private void SpawnTwoCubeObstacles()
    {
        for (int i = 0; i < 2; i++)
        {
            RunPlatform cubeObstacle = Managers.ObjectPool.GetObject(cubeObstaclePrefab.gameObject).GetComponent<RunPlatform>();
            cubeObstacle.Move(cubeObstaclePos + cubeObstacleDistance * i, Vector2.left, true);
            cubeObstacles.Add(cubeObstacle);
        }
    }
}
