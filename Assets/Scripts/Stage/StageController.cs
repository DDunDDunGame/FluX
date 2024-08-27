using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    [SerializeField] private Define.Stage currentStage = Define.Stage.None;
    [SerializeField] private Define.Stage testStage;
    [SerializeField] private float changeTime = 0.2f;
    [SerializeField] private float initY;
    [SerializeField] private RectTransform[] flyTransforms;
    private Dictionary<Define.Stage, BaseStage> stageDict;
    private List<IStageAttachment> attachments;

    private const int ROUND_STAGE_MAX = 5;
    private int roundStageCount = 0;
    private Transform map;

    public Player Player { get; private set; }
    public TextMeshProUGUI TimeText { get; private set; }
    public TextMeshProUGUI ScoreText { get; private set; }
    public RoundImage RoundImage { get; private set; }
    private float scoreTimer = 0f;
    private float scoreTime = 1f;
    private VolumeHelper changeVolume;
    // 추후 아이템 배치를 위한 변수들
    [SerializeField] private Fuel fuelPrefab;
    [SerializeField] private Bullet bulletPrefab;
    private float bulletItemTimer = 2f;
    private float bulletItemTime = 5f;

    private void Awake()
    {
        InitVariables();
        InitDict();
        InitAttachments();

        Managers.Game.GameOverAction -= FinishGame;
        Managers.Game.GameOverAction += FinishGame;
    }

    private void Start()
    {
        FlyUp(InitStageChange);
    }

    private void FlyUp(TweenCallback action)
    {
        transform.position = new Vector3(transform.position.x, initY, transform.position.z);
        transform.DOMoveY(transform.position.y + 10, 0.5f).SetEase(Ease.OutQuad).OnComplete(action);
        
        foreach(RectTransform child in flyTransforms)
        {
            float rectInitY = child.rect.y;
            child.DOAnchorPosY(rectInitY - 1000, 0);
            child.DOAnchorPosY(rectInitY - 60, 0.5f).SetEase(Ease.OutQuad);
        }
    }

    private void InitStageChange()
    {
        ChangeStage(SetRandomStage());
        Managers.Game.Play();
    }

    private void InitDict()
    {
        stageDict = new()
        {
            { Define.Stage.Shooting, new ShootingStage(this) },
            { Define.Stage.Mouse, new MouseStage(this) },
            { Define.Stage.Run, new RunStage(this) },
            { Define.Stage.Jump, new JumpStage(this) },
            { Define.Stage.Orbit, new OrbitStage(this) },
            { Define.Stage.Barrier, new BarrierStage(this) },
            { Define.Stage.Boss, new BossStage(this) }
        };
    }

    private void InitAttachments()
    {
        attachments = new List<IStageAttachment>();
        Util.FindChild(gameObject, "Attachments").GetComponentsInChildren(attachments);
    }

    private void InitVariables()
    {
        map = Util.FindChild<Transform>(gameObject, "Map");
        Player = Util.FindChild<Player>(gameObject, "Player", true);
        TimeText = Util.FindChild<TextMeshProUGUI>(gameObject, "TimeText", true);
        ScoreText = Util.FindChild<TextMeshProUGUI>(gameObject, "ScoreText", true);
        RoundImage = Util.FindChild<RoundImage>(gameObject, "RoundImage", true);
        changeVolume = new(Player, Util.FindChild<Volume>(gameObject, "ChangeVolume", true));
    }

    private void Update()
    {
        if (currentStage == Define.Stage.None) { return; }

        if (stageDict[currentStage].IsEnd())
        {
            ChangeStage(SetRandomStage());
        }
        else
        {
            stageDict[currentStage].Update();
        }

        UpdateScore();
    }

    private void UpdateScore()
    {
        if (Managers.Game.IsPlaying)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= scoreTime)
            {
                scoreTimer = 0f;
                Managers.Game.AddScore(100);
            }
        }
        ScoreText.text = Managers.Game.Score.ToString();
    }

    private Define.Stage SetRandomStage()
    {
        if(roundStageCount < ROUND_STAGE_MAX)
        {
            roundStageCount++;
            RoundImage.SetRoundImage(roundStageCount);
            int randomStart = (int)Define.Stage.None + 1;
            int randomEnd = (int)Define.Stage.Boss;
            return (Define.Stage)UnityEngine.Random.Range(randomStart, randomEnd);
        }
        else
        {
            roundStageCount = 0;
            return Define.Stage.Boss;
        }
    }

    public void ChangeStage(Define.Stage stage)
    {
        Player.Sprite.enabled = false;
        if (currentStage != Define.Stage.None)
        {
            stageDict[currentStage].Destroy();
            if(Player.Stat.HitCount == 0)
            {
                EnableFuelItem();
            }
            Player.Stat.ResetHitCount();
            foreach (IStageAttachment attachment in attachments)
            {
                attachment.ExitStage(currentStage);
            }
        }

        Managers.Game.Pause();
        ChangingStage(stage);
    }

    private void ChangingStage(Define.Stage stage)
    {
        currentStage = Define.Stage.None;
        foreach (IStageAttachment attachment in attachments)
        {
            attachment.EnterStage(currentStage);
        }

        void Change() { FinishChangingStage(stage); }
        void Disable() { changeVolume.DisableSmooth(changeTime / 3, Change); }
        void Stay() { changeVolume.SetActive(true, changeTime / 3, Disable); }
        changeVolume.EnableSmooth(changeTime / 3, Stay);
    }

    private void FinishChangingStage(Define.Stage stage)
    {
        foreach (IStageAttachment attachment in attachments)
        {
            attachment.ExitStage(currentStage);
        }
        currentStage = stage;
        stageDict[currentStage].Initialize();
        foreach (IStageAttachment attachment in attachments)
        {
            attachment.EnterStage(currentStage);
        }
        Player.Sprite.enabled = true;
        Managers.Game.Resume();
    }

    private void FinishGame()
    {
        SoundManager.Instance.PlaySound2D("SFX GameOver");
        if (currentStage != Define.Stage.None)
        {
            stageDict[currentStage].Destroy();
            foreach (IStageAttachment attachment in attachments)
            {
                attachment.ExitStage(currentStage);
            }
        }
        currentStage = Define.Stage.None;
        gameObject.SetActive(false);
    }

    public GameObject CreateMap(GameObject obj, Vector3 pos)
    {
        return Instantiate(obj, pos, Quaternion.identity, map);
    }

    public void EnableFuelItem()
    {
        Fuel fuel = Managers.ObjectPool.GetObject(fuelPrefab.gameObject).GetComponent<Fuel>();
        fuel.Launch();
    }

    public void TryEnableBulletItem(float stageTime)
    {
        if (stageTime >= bulletItemTimer && stageTime <= bulletItemTimer + bulletItemTime)
        {
            bulletItemTimer = (bulletItemTime + bulletItemTimer) % 10;
            Bullet bullet = Managers.ObjectPool.GetObject(bulletPrefab.gameObject).GetComponent<Bullet>();
            bullet.Launch();
        }
    }

    public void DestroyMap()
    {
        foreach (Transform child in map)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetTimeText(float time)
    {
        time = 10f - time;
        time = Mathf.Clamp(time, 0f, 10f);
        int timeInt = Mathf.RoundToInt(time);
        TimeText.text = timeInt.ToString();
    }
}
