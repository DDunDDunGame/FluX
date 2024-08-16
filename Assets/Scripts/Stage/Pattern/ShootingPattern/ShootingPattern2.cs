using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootingPattern2 : MonoBehaviour, IPattern
{
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private GuidedMissile guidedMissilePrefab;
    [SerializeField] private float spawnX = 11f;

    private Player player;
    private float spawnTimer = 0f;

    public void Initialize(Player player)
    {
        this.player = player;
        spawnTimer = Time.time;
    }

    public void OnUpdate()
    {
        if (Time.time >= spawnTimer + spawnInterval)
        {
            spawnTimer = Time.time;
            SpawnGuidedMissile();
        }
    }

    private void SpawnGuidedMissile()
    {
        Vector2 spawnPoint = new Vector2(spawnX, Random.Range(-3f, 3f));
        GuidedMissile guidedMissile = Instantiate(guidedMissilePrefab, spawnPoint, Quaternion.identity, transform);
        guidedMissile.Launch(player.transform);
    }

    public void Destroy()
    {

    }
}
