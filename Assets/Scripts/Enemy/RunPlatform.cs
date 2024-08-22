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
    private bool isEnemy;

    public void Move(Vector2 pos, Vector2 dir, bool isEnemy = false)
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        transform.position = pos;
        rigid.velocity = dir.normalized * speed;
        this.isEnemy = isEnemy;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isEnemy) { return; }

        if (collision.gameObject.TryGetComponent(out IDamageable damageable) && collision.gameObject.CompareTag("Player"))
        {
            damageable.TakeDamage(damage);
        }
    }
}
