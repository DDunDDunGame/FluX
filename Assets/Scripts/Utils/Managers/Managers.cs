using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    private ObjectPoolManager _objectPool = new ObjectPoolManager();

    public static ObjectPoolManager ObjectPool { get { return Instance._objectPool; } }
}
