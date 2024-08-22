using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour, IStageAttachment, IDamageable
{
    private Define.Stage currentStage;
    public SpriteRenderer Sprite { get; private set; }
    public Rigidbody2D Rigid { get; private set; }
    public Collider2D Coll { get; private set; }
    public Slider HpSlider { get; private set; }
    public LayoutGroupHelper BulletGroup { get; private set; }
    public PlayerStat Stat { get; private set; }
    public PlayerActions Actions { get; private set; }
    public PlayerVolume HitVolume { get; private set; }
    public PlayerVolume InvicibleVolume { get; private set; }
    private Dictionary<Define.Stage, PlayerOnStage> onStageDict;

    #region Sprite
    [field: SerializeField]
    public Sprite CircleHit { get; private set; }
    [field: SerializeField]
    public Sprite CircleIdle{ get; private set; }
    [field: SerializeField]
    public Sprite SquareHit { get; private set; }
    [field: SerializeField]
    public Sprite SquareIdle { get; private set; }
    [field: SerializeField]
    public Sprite ShootingSprite { get; private set; }
    #endregion

    private void Awake()
    {
        InitVariables();
    }

    private void Update()
    {
        if(currentStage != Define.Stage.None)
        {
            onStageDict[currentStage].OnUpdate();
        }
    }

    private void InitVariables()
    {
        currentStage = Define.Stage.None;
        Sprite = GetComponent<SpriteRenderer>();
        Rigid = GetComponent<Rigidbody2D>();
        Coll = GetComponent<Collider2D>();
        HpSlider = Util.FindChild<Slider>(gameObject, "HpSlider", true);
        BulletGroup = Util.FindChild<LayoutGroupHelper>(gameObject, "BulletGroup", true);
        Stat = new PlayerStat(this);
        Actions = new PlayerActions();
        HitVolume = new PlayerVolume(this, Util.FindChild<Volume>(gameObject, "HitVolume"));
        InvicibleVolume = new PlayerVolume(this, Util.FindChild<Volume>(gameObject, "InvicibleVolume"));
        onStageDict = new Dictionary<Define.Stage, PlayerOnStage>
        {
            { Define.Stage.Shooting, new PlayerOnShootingStage(this) },
            { Define.Stage.Mouse, new PlayerOnMouseStage(this) },
            { Define.Stage.Run, new PlayerOnRunStage(this) },
            { Define.Stage.Jump, new PlayerOnJumpStage(this) },
            { Define.Stage.Orbit, new PlayerOnOrbitStage(this) },
            { Define.Stage.Barrier, new PlayerOnBarrierStage(this) },
            { Define.Stage.Boss, new PlayerOnBossStage(this) }
        };
    }

    public void ChangeStage(Define.Stage stage)
    {
        if(currentStage != Define.Stage.None)
        {
            onStageDict[currentStage].OnExit();
        }
        currentStage = stage;
        onStageDict[currentStage].OnEnter();
    }

    public void TakeDamage(float damage)
    {
        SoundManager.Instance.PlaySound2D("SFX Player Hit");
        Stat.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IItem item))
        {
            item.Use(Stat);
        }
    }

    public void ChangeSpriteByHit(float time)
    {
        StartCoroutine(ChangeSpriteCoroutine(time));
    }

    private IEnumerator ChangeSpriteCoroutine(float time)
    {
        Sprite original = Sprite.sprite;
        if (original == CircleIdle)
        {
            Sprite.sprite = CircleHit;
        }
        else if (original == SquareIdle)
        {
            Sprite.sprite = SquareHit;
        }
        yield return new WaitForSeconds(time);
        Sprite.sprite = original;
    }
}
