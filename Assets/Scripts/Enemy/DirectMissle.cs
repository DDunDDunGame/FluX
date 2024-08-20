using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DirectMissle : Poolable
{
    private Rigidbody2D rigid;

    public void Launch(float speed)
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        rigid.velocity = Vector2.left * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable) && collision.CompareTag("Player"))
        {
            damageable.TakeDamage(10);
            ReturnToPool();
        }
    }
}