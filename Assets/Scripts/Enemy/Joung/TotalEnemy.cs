using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalEnemy : MonoBehaviour, IDamageable
{
    public void TakeDamage(float atk)
    {
        if (transform.name.Contains("Boss"))
        {
            SoundManager.Instance.PlaySound2D("SFX Boss Damage");
            Managers.Game.AddScore(100);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player>().TakeDamage(5);
        }

        if(collision.transform.tag == "Barrier")
        {
            if (transform.tag == "BarrierRay")
            {
                return;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
