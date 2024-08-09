using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : BaseStage
{
    public BossStage(StageController controller) : base(controller)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("BossStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("BossStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        Debug.Log("BossStage Destroy");
    }
}
