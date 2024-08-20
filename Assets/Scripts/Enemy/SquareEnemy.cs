using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SquareEnemy : Poolable, IDamageable
{
    [SerializeField] private float damage = 10f;
    private Rigidbody2D rigid;

    public void Move(float speed)
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        rigid.velocity = Vector2.left * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && collision.CompareTag("Player"))
        {
            damageable.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        ReturnToPool();
    }
}
