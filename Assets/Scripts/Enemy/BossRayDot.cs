using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossRayDot : MonoBehaviour
{
    private Sequence mySequence;

    private void Start()
    {
        mySequence = DOTween.Sequence()
        .SetAutoKill(false)
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
        })
        .OnPlay(() =>
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        })
        .Append(transform.DOScaleY(2, 0.1f))
        .Append(transform.DOScaleY(1, 0.1f))
        .Append(transform.DOScaleY(2, 0.1f))
        .Append(transform.DOScaleY(1, 0.1f))
        .Append(transform.DOScaleY(2, 0.1f))
        .Append(transform.DOScaleY(1, 0.1f))
        .Append(transform.DOScaleY(2, 0.1f))
        .Append(transform.DOScaleY(1, 0.1f))
        .Append(transform.DOScaleY(0, 0.2f))
        .SetDelay(0.1f);
    }

    private void OnEnable()
    {
        mySequence.Restart();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player>().TakeDamage(5);
        }
    }
}
