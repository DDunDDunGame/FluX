using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierStage : BaseStage
{
    public BarrierStage(StageController controller) : base(controller)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("BarrierStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("BarrierStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        Debug.Log("BarrierStage Destroy");
    }
}
