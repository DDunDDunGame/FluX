using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStage : BaseStage
{
    public RunStage(StageController controller) : base(controller)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("RunStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("RunStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        Debug.Log("RunStage Destroy");
    }
}
