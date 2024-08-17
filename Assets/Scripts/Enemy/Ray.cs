using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class Ray : Poolable
{
    [SerializeField] private SpriteRenderer warning;
    [SerializeField] private float warningTime = 1f;
    [SerializeField] private float warningMaxAlpha = 0.7f;
    [SerializeField] private float rayTime = 5f;
    [SerializeField] private float rayMaxAlpha = 0.7f;
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
        coll.enabled = false;
        rayScale = scale;
        transform.localScale = new Vector3(rayScale, transform.localScale.y, transform.localScale.z);
        StartCoroutine(ShowWarning());
    }

    private IEnumerator ShowWarning()
    {
        warning.enabled = true;
        warning.color = new Color(1f, 0f, 0f, 0f);
        float timer = 0f;
        while (timer < warningTime)
        {
            timer += Time.deltaTime;
            warning.color = new Color(1f, 0f, 0f, warningMaxAlpha * (timer / warningTime));
            yield return null;
        }
        warning.enabled = false;
        StartCoroutine(ShowRay());
    }

    private IEnumerator ShowRay()
    {
        sprite.enabled = true;
        sprite.color = new Color(1f, 1f, 1f, rayMaxAlpha);
        float timer = 0f;
        while (timer < rayTime)
        {
            timer += Time.deltaTime;
            float xScale = rayScale * (1f - (timer / rayTime));
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);  
            yield return null;
        }
        sprite.enabled = false;
    }
}
