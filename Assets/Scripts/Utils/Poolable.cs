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

        try
        {
            Pool.Release(gameObject);
        }
        catch (System.Exception) { }
    }

}
