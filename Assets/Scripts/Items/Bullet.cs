using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Poolable, IItem
{
    [SerializeField] private int bullet = 1;
    [SerializeField] private Vector2 startPos = new(9.5f, 0f);

    [SerializeField] private float amplitude = 1.0f; // 사인파의 진폭
    [SerializeField] private float frequency = 1.0f; // 사인파의 주파수
    [SerializeField] private float speed = 1.0f; // 이동 속도

    private bool isLaunched = false;
    private float timer;
    private ParticleSystem useEffect;
    private SpriteRenderer sprite;

    private void Awake()
    {
        Managers.Game.GameOverAction -= ReturnToPool;
        Managers.Game.GameOverAction += ReturnToPool;
        useEffect = Util.FindChild<ParticleSystem>(gameObject, "UseEffect");
        sprite = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        if (isLaunched)
        {
            timer += Time.fixedDeltaTime;
            Move();
        }
    }

    private void Move()
    {
        if (transform.position.x < -9.5f)
        {
            isLaunched = false;
            ReturnToPool();
        }
        float x = startPos.x + -1 * timer * speed;
        float y = startPos.y + Mathf.Sin(timer * frequency) * amplitude;
        transform.position = new Vector2(x, y);
    }

    public void Launch()
    {
        SoundManager.Instance.PlaySound2D("SFX Potion Create");
        transform.position = startPos;
        isLaunched = true;
        sprite.color = Color.white;
        timer = 0f;
    }

    public void Use(PlayerStat target)
    {
        if(isLaunched == false) { return; }
        SoundManager.Instance.PlaySound2D("SFX Bullet Get");
        target.RestoreBullet(bullet);
        isLaunched = false;
        StartCoroutine(EffectCoroutine());
    }

    private IEnumerator EffectCoroutine()
    {
        useEffect.Play();
        sprite.DOFade(0f, useEffect.main.duration);
        yield return new WaitForSeconds(useEffect.main.duration);
        useEffect.Stop();
        ReturnToPool();
    }
}