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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable) && collision.CompareTag("Player"))
        {
            damageable.TakeDamage(10);
        }
    }
}
