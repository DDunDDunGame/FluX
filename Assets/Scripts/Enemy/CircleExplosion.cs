using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CircleExplosion : Poolable
{
    private List<Rigidbody2D> fragments = new();
    [SerializeField] private float fragmentRadius = 0.5f;
    [SerializeField] private float explosionSpeed = 5f;

    public void MakeExplosion()
    {
        if(fragments.Count == 0)
        {
            Rigidbody2D[] rigidbodies = GetComponentsInChildren<Rigidbody2D>();
            fragments.AddRange(rigidbodies);
        }
        ResetFragments();

        Vector3 center = transform.position;
        foreach(Rigidbody2D fragment in fragments)
        {
            Vector2 dir = (fragment.transform.position - center).normalized;
            fragment.velocity = dir * explosionSpeed;
            fragment.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        }
    }

    private void ResetFragments()
    {
        float angle = 360f / fragments.Count;
        for(int i = 0; i < fragments.Count; i++)
        {
            float radian = angle * i * Mathf.Deg2Rad;
            Vector3 direction = new(Mathf.Cos(radian), Mathf.Sin(radian), 0);
            fragments[i].transform.position = transform.position + direction * fragmentRadius;
        }
    }
}
