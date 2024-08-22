using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    private ObjectPoolManager _objectPool = new ObjectPoolManager();
    private GameManager _game = new GameManager();

    public static ObjectPoolManager ObjectPool { get { return Instance._objectPool; } }
    public static GameManager Game { get { return Instance._game; } }
}
