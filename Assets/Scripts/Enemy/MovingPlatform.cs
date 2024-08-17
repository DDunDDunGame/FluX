using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : Poolable
{
    private Rigidbody2D rigid;

    public void Move(Vector2 pos, Vector2 dir)
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        rigid.position = pos;
        rigid.velocity = dir;
    }
}
