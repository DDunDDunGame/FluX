using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPattern1 : MonoBehaviour, IPattern
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private SquareObstacle pillarPrefab;
    [SerializeField] private SquareEnemy enemyPrefab;

    private List<SquareObstacle> pillars = new List<SquareObstacle>();
    private List<SquareEnemy> enemies = new List<SquareEnemy>();
    private float spawnTimer = 0f;

    public void Initialize(Player player)
    {
        spawnTimer = Time.time;
    }

    public void OnUpdate()
    {
        if (Time.time >= spawnTimer + spawnInterval)
        {
            SpawnPillar();
            spawnTimer = Time.time;
        }
    }

    private void SpawnPillar()
    {
        float randomY = Random.Range(-3f, 3f);
        spawnPoint.y = randomY;
        Vector3 topPillarPoint = new Vector3(spawnPoint.x, randomY / 2 + 3, 0);
        Vector3 bottomPillarPoint = new Vector3(spawnPoint.x, randomY / 2 - 3, 0);

        SquareObstacle topPillar = Managers.ObjectPool.GetObject(pillarPrefab.gameObject).GetComponent<SquareObstacle>();
        SquareObstacle bottomPillar = Managers.ObjectPool.GetObject(pillarPrefab.gameObject).GetComponent<SquareObstacle>();
        topPillar.transform.position = topPillarPoint;
        bottomPillar.transform.position = bottomPillarPoint;
        topPillar.transform.localScale = new Vector3(1, 4 - randomY, 1);
        bottomPillar.transform.localScale = new Vector3(1, 4 + randomY, 1);

        SquareEnemy enemy = Managers.ObjectPool.GetObject(enemyPrefab.gameObject).GetComponent<SquareEnemy>();
        enemy.transform.position = spawnPoint;


        topPillar.Move(moveSpeed);
        bottomPillar.Move(moveSpeed);
        enemy.Move(moveSpeed);

        pillars.Add(topPillar);
        pillars.Add(bottomPillar);
        enemies.Add(enemy);
    }

    public void Destroy()
    {
        foreach(SquareObstacle pillar in pillars)
        {
            pillar.ReturnToPool();
        }
        foreach(SquareEnemy enemy in enemies)
        {
            enemy.ReturnToPool();
        }
    }
}
