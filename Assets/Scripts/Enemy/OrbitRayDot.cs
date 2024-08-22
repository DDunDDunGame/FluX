using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrbitRayDot : MonoBehaviour
{
    private Sequence mySequence;
    private Vector3 currentScale;
    private Player player;
    private SpriteRenderer currentSr;
    private bool playerFlag = false;
    private int attackCount = 1;

    private void Start()
    {
        currentScale = transform.localScale;
        currentSr = transform.GetComponent<SpriteRenderer>();
        playerFlag = false;
        attackCount = 1;

        mySequence = DOTween.Sequence()
        .SetAutoKill(false)
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
            playerFlag = false;
            attackCount = 1;
        })
        .OnPlay(() =>
        {
            transform.localScale = currentScale;
            transform.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 0);
        })
        .Append(transform.GetComponent<SpriteRenderer>().DOFade(1, 3f).SetEase(Ease.Linear))
        .Append(transform.DOScaleY(1.3f, 0))
        .Join(transform.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1), 0))
        .Append(transform.DOScaleY(0, 1).SetEase(Ease.Linear))
        .Join(transform.GetComponent<SpriteRenderer>().DOFade(0, 1).SetEase(Ease.Linear))
        .SetDelay(0.2f);
    }

    private void Update()
    {
        if(attackCount > 0 && playerFlag && currentSr.color.b == 1)
        {
            Debug.Log("Attack Start");
            player.TakeDamage(10);
            attackCount--;
        }
    }

    private void OnEnable()
    {
        mySequence.Restart();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerFlag = true;
            player = collision.transform.GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            playerFlag = false;
        }
    }
}
