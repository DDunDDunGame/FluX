using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class RayEnemy : Poolable
{
    [SerializeField] private SpriteRenderer warning;
    private float warningTime = 1f;
    [SerializeField] private float warningMaxAlpha = 0.7f;
    private float rayOnTime = 0.3f;
    private float rayRemainTime = 0.5f;
    [SerializeField] private float rayMaxAlpha = 0.7f;
    [SerializeField] private float damage = 10f;
    private float rayScale;
    private SpriteRenderer sprite;
    private Collider2D coll;

    public void ReadyRay(float scale)
    {
        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        if (coll == null)
        {
            coll = GetComponent<Collider2D>();
        }
        sprite.enabled = false;
        coll.enabled = false;
        rayScale = scale;
        transform.localScale = new Vector3(transform.localScale.x, rayScale, transform.localScale.z);
        StartCoroutine(ShowWarning());
    }

    private IEnumerator ShowWarning()
    {
        warning.enabled = true;
        warning.color = new Color(1f, 1f, 1f, 0f);
        float timer = 0f;
        while (timer < warningTime)
        {
            timer += Time.deltaTime;
            warning.color = new Color(1f, 1f, 1f, warningMaxAlpha * (timer / warningTime));
            yield return null;
        }
        warning.enabled = false;
        StartCoroutine(ShowRay());
    }

    private IEnumerator ShowRay()
    {
        sprite.enabled = true;
        coll.enabled = true;
        sprite.color = new Color(1f, 1f, 1f, rayMaxAlpha);
        float timer = 0f;
        while (timer < rayRemainTime)
        {
            timer += Time.deltaTime;
            float yScale = rayScale * (timer / rayRemainTime);
            transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);
            yield return null;
        }
        timer = 0f;
        while (timer < rayRemainTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        coll.enabled = false;
        while (timer < rayOnTime)
        {
            timer += Time.deltaTime;
            float yScale = rayScale * (1f - (timer / rayOnTime));
            transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);  
            yield return null;
        }
        sprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable) && collision.gameObject.CompareTag("Player"))
        {
            damageable.TakeDamage(damage);
        }
    }
}
