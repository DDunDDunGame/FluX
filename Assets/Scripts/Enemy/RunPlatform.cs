using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RunPlatform : Poolable
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 10f;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private bool isEnemy = false;

    public void Move(Vector2 pos, Vector2 dir, bool isEnemy = false)
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        transform.position = pos;
        rigid.velocity = dir.normalized * speed;
        this.isEnemy = isEnemy;
        SetSprite();
    }

    private void SetSprite()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isEnemy) { return; }

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage);
    
    }
}
