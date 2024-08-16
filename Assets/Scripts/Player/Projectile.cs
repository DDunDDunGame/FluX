using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Poolable
{
    private Rigidbody2D rigid;
    private float moveSpeed = 10f;
    private float lifeTime = 2f;

    public void Launch(Vector2 direction)
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = direction * moveSpeed;
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Enemy"))
        {
            return;
        }

        if(collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(1);
            ReturnToPool();
        }
    }
}
