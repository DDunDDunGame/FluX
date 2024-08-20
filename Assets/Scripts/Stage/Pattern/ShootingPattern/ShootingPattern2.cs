using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootingPattern2 : MonoBehaviour, IPattern
{
    [SerializeField] private float guidedSpawnInterval = 1f;
    [SerializeField] private float directSpawnInterval = 2f;
    [SerializeField] private GuidedMissile guidedMissilePrefab;
    [SerializeField] private DirectMissle directMissilePrefab;
    [SerializeField] private float spawnX = 11f;

    private Player player;
    private float guidedTimer;
    private float directTimer;
    private List<GuidedMissile> guidedMissiles = new();
    private List<DirectMissle> directMissles = new();

    public void Initialize(Player player)
    {
        this.player = player;
        guidedTimer = Time.time;
        directTimer = Time.time;
    }

    public void OnUpdate()
    {
        if (Time.time >= guidedTimer + guidedSpawnInterval)
        {
            guidedTimer = Time.time;
            SpawnGuidedMissile();
        }
        if(Time.time >= directTimer + directSpawnInterval)
        {
            directTimer = Time.time;
            SpawnDirectMissile();
        }
    }

    private void SpawnGuidedMissile()
    {
        Vector2 spawnPoint = new(spawnX, Random.Range(-3f, 3f));
        GuidedMissile guidedMissile = Managers.ObjectPool.GetObject(guidedMissilePrefab.gameObject).GetComponent<GuidedMissile>();
        guidedMissile.transform.position = spawnPoint;
        guidedMissile.Launch(player.transform);
        guidedMissiles.Add(guidedMissile);
    }

    private void SpawnDirectMissile()
    {
        Vector2 spawnPoint = new(spawnX, Random.Range(-3f, 3f));
        DirectMissle directMissile = Managers.ObjectPool.GetObject(directMissilePrefab.gameObject).GetComponent<DirectMissle>();
        directMissile.transform.position = spawnPoint;
        directMissile.Launch(5f);
        directMissles.Add(directMissile);
    }

    public void Destroy()
    {
        foreach(GuidedMissile guidedMissile in guidedMissiles)
        {
            guidedMissile.ReturnToPool();
        }
        foreach(DirectMissle directMissle in directMissles)
        {
            directMissle.ReturnToPool();
        }
    }
}
