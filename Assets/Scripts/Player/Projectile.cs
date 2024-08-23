using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Poolable
{
    private ParticleSystem hitEffect;
    private Rigidbody2D rigid;
    private Collider2D coll;
    private SpriteRenderer sprite;
    private TrailRenderer trail;
    private float moveSpeed = 10f;
    private float lifeTime = 2f;
    private Coroutine returnCoroutine;

    private void Awake()
    {
        hitEffect = Util.FindChild<ParticleSystem>(gameObject, "HitEffect");
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
    }

    public void Launch(Vector2 direction)
    {
        SoundManager.Instance.PlaySound2D("SFX Shooting");
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        coll.enabled = true;
        sprite.enabled = true;
        trail.Clear();
        rigid.velocity = direction * moveSpeed;
        returnCoroutine = StartCoroutine(ReturnToPoolAfterLifeTime());
    }

    private IEnumerator ReturnToPoolAfterLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        ReturnToPool();
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
        }
        rigid.velocity = Vector2.zero;
        StopCoroutine(returnCoroutine);
        coll.enabled = false;
        sprite.enabled = false;
        StartCoroutine(HitEffectCoroutine());
    }

    private IEnumerator HitEffectCoroutine()
    {
        hitEffect.Play();
        while(hitEffect.isPlaying)
        {
            yield return null;
        }
        ReturnToPool();
    }
}
