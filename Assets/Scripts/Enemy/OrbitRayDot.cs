using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrbitRayDot : MonoBehaviour
{
    private Sequence mySequence;
    float screenY;
    float screenX;
    private bool flag = true;

    private void Start()
    {
        screenY = Camera.main.orthographicSize * 2;
        screenX = screenY / Screen.height * Screen.width;

        mySequence = DOTween.Sequence()
        .SetAutoKill(false)
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
        })
        .OnPlay(() =>
        {
            transform.localScale = new Vector3(2, screenX * 1.5f, 1);
            transform.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 0);
        })
        .Append(transform.GetComponent<SpriteRenderer>().DOFade(1, 3f).SetEase(Ease.Linear))
        .Append(transform.DOScaleX(3, 0))
        .Join(transform.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1), 0))
        .Append(transform.DOScaleX(0, 1).SetEase(Ease.Linear))
        .Join(transform.GetComponent<SpriteRenderer>().DOFade(0, 1).SetEase(Ease.Linear))
        .SetDelay(0.5f);
    }

    private void OnEnable()
    {
        mySequence.Restart();
    }
}
