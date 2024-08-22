using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformEnemy : Poolable
{
    [SerializeField] private float damage = 10f;
    private Rigidbody2D rigid;

    public void Move(Vector2 pos, Vector2 dir)
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        transform.position = pos;
        rigid.velocity = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable) && collision.CompareTag("Player"))
        {
            damageable.TakeDamage(damage);
        }
    }
}
