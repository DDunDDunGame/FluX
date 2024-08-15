using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private Define.Stage currentStage = Define.Stage.None;
    [SerializeField] private Define.Stage testStage;
    private Dictionary<Define.Stage, BaseStage> stageDict;
    private List<IStageAttachment> attachments;

    private const int ROUND_STAGE_MAX = 5;
    private int roundStageCount = 0;

    // ÃßÈÄ ¾ÆÀÌÅÛ ¹èÄ¡¸¦ À§ÇÑ º¯¼öµé
    public GameObject Fuel { get; private set; }
    public GameObject Bullets { get; private set; }

    private void Awake()
    {
        InitDict();
        InitAttachments();
        Fuel = Util.FindChild(gameObject, "Fuel");
        Bullets = Util.FindChild(gameObject, "Bullets");
    }

    private void Start()
    {
        // ì—ë””í„°ì—ì„œëŠ” í…ŒìŠ¤íŠ¸ë¥¼ ìœ„í•´ ì›í•˜ëŠ” ìŠ¤í…Œì´ì§€ë¡œ ì§€ì •, ë¹Œë“œ ì‹œ ëœë¤ ìŠ¤í…Œì´ì§€
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

    private void Update()
    {
        if (currentStage == Define.Stage.None) { return; }

        stageDict[currentStage].Update();

        //if (stageDict[currentStage].IsEnd())
        //{
        //    ChangeStage(SetRandomStage());
        //}
        //else
        //{
        //    stageDict[currentStage].Update();
        //}
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
        }

        currentStage = stage;
        stageDict[currentStage].Initialize();
        foreach (IStageAttachment attachment in attachments)
        {
            attachment.ChangeStage(currentStage);
        }
    }
}
