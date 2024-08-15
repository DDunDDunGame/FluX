using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Poolable, IItem
{
    [SerializeField] private int bullet = 10;

    public void Use(PlayerStat target)
    {
        target.RestoreBullet(bullet);
        ReturnToPool();
    }
}