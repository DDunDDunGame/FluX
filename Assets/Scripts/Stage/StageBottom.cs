using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBottom : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out IDamageable damageable) && collision.gameObject.CompareTag("Player"))
        {
            damageable.TakeDamage(damage);
        }
    }
}
