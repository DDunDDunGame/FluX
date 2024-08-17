using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePattern2 : MonoBehaviour, IPattern
{
    [SerializeField] private float interval = 1f;
    [SerializeField] private List<Rebar> rebarPrefabs = new();
    private List<Rebar> rebars = new();
    private float timer = 0f;

    public void Initialize(Player player)
    {
        timer = Time.time;
    }

    public void OnUpdate()
    {
        if(Time.time >= timer + interval)
        {
            timer = Time.time;
            SpawnRandomRebar();
        }
    }

    private void SpawnRandomRebar()
    {
        int index = Random.Range(0, rebarPrefabs.Count);
        Rebar rebar = Managers.ObjectPool.GetObject(rebarPrefabs[index].gameObject).GetComponent<Rebar>();
        rebar.Launch();
        rebars.Add(rebar);
    }

    public void Destroy()
    {
        foreach(Rebar rebar in rebars)
        {
            rebar.ReturnToPool();
        }
        rebars.Clear();
    }
}
