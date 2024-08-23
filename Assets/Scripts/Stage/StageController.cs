using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

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
            child.DOAnchorPosY(rectInitY - 1300, 0);
            child.DOAnchorPosY(rectInitY, 0.5f).SetEase(Ease.OutQuad);
        }
    }

    private void InitStageChange()
    {
#if UNITY_EDITOR
        ChangeStage(testStage);
#else
        ChangeStage(SetRandomStage());
#endif
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
    }

    private Define.Stage SetRandomStage()
    {
        if(roundStageCount < ROUND_STAGE_MAX)
        {
            roundStageCount++;
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
        if (currentStage != Define.Stage.None)
        {
            stageDict[currentStage].Destroy();
            if(Player.Stat.HitCount == 0)
            {
                EnableFuelItem();
            }
            Player.Stat.ResetHitCount();
        }

        Managers.Game.Pause();
        ChangingStage(stage);
    }

    private void ChangingStage(Define.Stage stage)
    {
        currentStage = Define.Stage.None;

        void Change() { FinishChangingStage(stage); }
        void Disable() { changeVolume.DisableSmooth(changeTime / 3, Change); }
        void Stay() { changeVolume.SetActive(true, changeTime / 3, Disable); }
        changeVolume.EnableSmooth(changeTime / 3, Stay);
    }

    private void FinishChangingStage(Define.Stage stage)
    {
        currentStage = stage;
        stageDict[currentStage].Initialize();
        foreach (IStageAttachment attachment in attachments)
        {
            attachment.ChangeStage(currentStage);
        }
        Managers.Game.Resume();
    }

    private void FinishGame()
    {
        if (currentStage != Define.Stage.None)
        {
            stageDict[currentStage].Destroy();
        }
        currentStage = Define.Stage.None;
        foreach (IStageAttachment attachment in attachments)
        {
            attachment.ChangeStage(currentStage);
        }
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
