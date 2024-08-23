using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class ObjectPoolManager
{
    private Dictionary<string, IObjectPool<GameObject>> poolDict;
    private Dictionary<string, GameObject> goDict;
    private string objectName;

    public ObjectPoolManager()
    {
        poolDict = new Dictionary<string, IObjectPool<GameObject>>();
        goDict = new Dictionary<string, GameObject>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        poolDict.Clear();
        goDict.Clear();
    }

    public void CreatePool(GameObject prefab)
    {
        if (!poolDict.ContainsKey(prefab.name))
        {
            goDict.Add(prefab.name, prefab);
            ObjectPool<GameObject> pool = new(CreatePoolObject, OnGet, OnRelease, OnDestroy);
            poolDict.Add(prefab.name, pool);
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        objectName = prefab.name;
        if (!poolDict.ContainsKey(objectName))
        {
            CreatePool(prefab);
        }
        return poolDict[objectName].Get();
    }

    public void DestroyPool(GameObject prefab)
    {
        if (poolDict.ContainsKey(prefab.name))
        {
            poolDict.Remove(prefab.name);
        }
    }

    private GameObject CreatePoolObject()
    {
        GameObject go = GameObject.Instantiate(goDict[objectName]);
        if(go.TryGetComponent(out Poolable poolable))
        {
            poolable.Pool = poolDict[objectName];
        }
        return go;
    }

    private void OnGet(GameObject go)
    {
        go.SetActive(true);
    }

    private void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }

    private void OnDestroy(GameObject go)
    {
        GameObject.Destroy(go);
    }
}
