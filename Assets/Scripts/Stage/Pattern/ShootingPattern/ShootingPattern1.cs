using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPattern1 : MonoBehaviour, IPattern
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private GameObject pillarPrefab;
    [SerializeField] private GameObject enemyPrefab;
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
        transform.Translate(moveSpeed * Vector3.left * Time.deltaTime);
    }

    private void SpawnPillar()
    {
        float randomY = Random.Range(-3f, 3f);
        spawnPoint.y = randomY;
        Vector3 topPillarPoint = new Vector3(spawnPoint.x, randomY / 2 + 3, 0);
        Vector3 bottomPillarPoint = new Vector3(spawnPoint.x, randomY / 2 - 3, 0);

        GameObject topPillar = Instantiate(pillarPrefab, topPillarPoint, Quaternion.identity, transform);
        GameObject bottomPillar = Instantiate(pillarPrefab, bottomPillarPoint, Quaternion.identity, transform);
        Instantiate(enemyPrefab, spawnPoint, Quaternion.identity, transform);

        topPillar.transform.localScale = new Vector3(1, 4 - randomY, 1);
        bottomPillar.transform.localScale = new Vector3(1, 4 + randomY, 1);
    }

    public void Destroy()
    {

    }
}
