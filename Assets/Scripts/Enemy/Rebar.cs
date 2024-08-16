using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rebar : Poolable
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector2 pos;
    [SerializeField] private Vector2 dir;
    private Rigidbody2D rigid;

    public void Launch()
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        transform.position = pos;
        rigid.velocity = dir.normalized * speed;
    }
}
