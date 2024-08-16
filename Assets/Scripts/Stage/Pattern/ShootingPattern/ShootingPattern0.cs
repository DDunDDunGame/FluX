using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootingPattern0 : MonoBehaviour, IPattern
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float enemyMoveSpeed = 3f;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private List<GameObject> obstacles = new List<GameObject>();
    [SerializeField] private Rigidbody2D enemy;
    [SerializeField] private Vector3 enemySpawnPoint;

    private List<GameObject> shuffledObstacles;
    private float obstacleInterval = 20f;
    private float spawnTimer = 0f;
    private List<GameObject> enemies = new List<GameObject>();

    public void Initialize(Player player)
    {
        shuffledObstacles = Util.Shuffle(obstacles);

        int count = 0;
        foreach (var obstacle in shuffledObstacles)
        {
            obstacle.transform.localPosition = new Vector3(obstacleInterval * count++, 0, 0);
        }
        spawnTimer = Time.time;
    }

    public void OnUpdate()
    {
        if (Time.time >= spawnTimer + spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = Time.time;
        }
        transform.Translate(moveSpeed * Vector3.left * Time.deltaTime);
    }

    public void Destroy()
    {
        shuffledObstacles.Clear();
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    private void SpawnEnemy()
    {
        enemySpawnPoint.y = Random.Range(-3f, 3f);
        Rigidbody2D enemyInstance = Instantiate(enemy, enemySpawnPoint, Quaternion.identity);
        enemyInstance.velocity = Vector2.left * enemyMoveSpeed;
        enemies.Add(enemyInstance.gameObject);
    }
}
