using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : Poolable, IItem
{
    [SerializeField] private int fuel = 10;

    public void Use(PlayerStat target)
    {
        target.RestoreHealth(fuel);
        ReturnToPool();
    }
}