using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private Define.Stage currentStage = Define.Stage.None;
    [SerializeField] private Define.Stage testStage;
    private Dictionary<Define.Stage, BaseStage> stageDict;
    private List<IStageAttachment> attachments;

    private const int ROUND_STAGE_MAX = 5;
    private int roundStageCount = 0;
    private Transform map;

    public Player Player { get; private set; }
    public TextMeshProUGUI TimeText { get; private set; }
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
    }

    private void Start()
    {
#if UNITY_EDITOR
        ChangeStage(testStage);
#else
        ChangeStage(SetRandomStage());
#endif
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
    }

    private void Update()
    {
        if (currentStage == Define.Stage.None) { return; }

        if (stageDict[currentStage].IsEnd())
        {
            ChangeStage(testStage);
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
            return (Define.Stage)Random.Range(randomStart, randomEnd);
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

        currentStage = stage;
        stageDict[currentStage].Initialize();
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
