using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    public void ReturnToPool()
    {
        if (Pool == null)
        {
            Destroy(gameObject);
            return;
        }
        Pool.Release(gameObject);
    }

}
