using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBottom : MonoBehaviour
{
    public bool canAttack = true;
    [SerializeField] private float damage = 10f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!canAttack)
        {
            return;
        }

        if(collision.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
            player.OnBottomHit?.Invoke();
        }
    }
}
