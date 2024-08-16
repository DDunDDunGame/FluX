using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePattern0 : MonoBehaviour, IPattern
{
    [SerializeField] private List<Transform> explosionPositions = new();
    [SerializeField] private CircleExplosion explosionPrefab;
    [SerializeField] private float explosionInterval = 0.5f;
    [SerializeField] private int explosionCountPerInterval = 2;
    private float explosionTimer = 0f;
    private List<CircleExplosion> explosions = new();

    public void Initialize(Player player)
    {
        explosionTimer = Time.time;
    }

    public void OnUpdate()
    {
        if(Time.time >= explosionTimer + explosionInterval)
        { 
            explosionTimer = Time.time;
            MakeExplosion();
        }
    }

    private void MakeExplosion()
    {
        for(int i = 0; i < explosionCountPerInterval; i++)
        {
            int randomIndex = Random.Range(0, explosionPositions.Count);

            GameObject explosionObject = Managers.ObjectPool.GetObject(explosionPrefab.gameObject);
            explosionObject.transform.position = explosionPositions[randomIndex].position;
            CircleExplosion explosion = explosionObject.GetComponent<CircleExplosion>();
            explosions.Add(explosion);
            explosion.MakeExplosion();
        }
    }

    public void Destroy()
    {
        foreach (CircleExplosion explosion in explosions)
        {
            explosion.ReturnToPool();
        }
        explosions.Clear();
        Debug.Log("MousePattern0 Destroy");
    }
}