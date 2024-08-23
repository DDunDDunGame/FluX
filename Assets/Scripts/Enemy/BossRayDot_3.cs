using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossRayDot_3 : MonoBehaviour
{
    private Sequence mySequence;
    private Vector2 firstPos;
    private float screenX;
    private float screenY;

    private void Start()
    {
        screenY = Camera.main.orthographicSize * 2;
        screenX = screenY / Screen.height * Screen.width;
        firstPos = transform.position;

        mySequence = DOTween.Sequence()
        .SetAutoKill(false)
        .OnComplete(() =>
        {
            transform.position = firstPos;
            gameObject.SetActive(false);
        })
        .OnPlay(() =>
        {
            transform.localScale = new Vector3(0.4f, 1.4f, 1);
            transform.GetComponent<SpriteRenderer>().color = new Color(255, 131, 131);
        })
        .Append(transform.DOMoveX(firstPos.x - 0.2f, 0.1f).SetEase(Ease.Linear))
        .Append(transform.DOMoveX(firstPos.x + 0.2f, 0.1f).SetEase(Ease.Linear))
        .Append(transform.DOMoveX(firstPos.x - 0.2f, 0.1f).SetEase(Ease.Linear))
        .Append(transform.DOMoveX(firstPos.x + 0.2f, 0.1f).SetEase(Ease.Linear))
        .Append(transform.DOMoveX(firstPos.x - 0.2f, 0.1f).SetEase(Ease.Linear))
        .Append(transform.DOMoveX(firstPos.x + 0.2f, 0.1f).SetEase(Ease.Linear))
        .Append(transform.DOMoveX(firstPos.x - 0.2f, 0.1f).SetEase(Ease.Linear))
        .Append(transform.DOMoveX(firstPos.x + 0.2f, 0.1f).SetEase(Ease.Linear))
        .Append(transform.DOMoveX(firstPos.x - 0.2f, 0.1f).SetEase(Ease.Linear))
        .Append(transform.DOMoveX(firstPos.x, 0.1f).SetEase(Ease.Linear))
        .Append(transform.DOScaleY(0, 0.5f).SetEase(Ease.Linear))
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
            if(transform.localScale.y > 1.2f) collision.transform.GetComponent<Player>().TakeDamage(5);
        }
    }
}
