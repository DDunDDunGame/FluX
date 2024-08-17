using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RunPlatform : Poolable
{
    [SerializeField] private float speed = 10f;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;

    public void Move(Vector2 pos, Vector2 dir, bool isEnemy = false)
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        transform.position = pos;
        rigid.velocity = dir.normalized * speed;
        SetSprite(isEnemy);
    }

    private void SetSprite(bool isEnemy)
    {
        if(sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        if(isEnemy)
        {
            sprite.color = Color.red;
        }
        else
        {
            sprite.color = Color.white;
        }
    }
}
