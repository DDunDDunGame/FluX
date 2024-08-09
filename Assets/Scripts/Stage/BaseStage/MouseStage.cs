using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStage : BaseStage
{
    public MouseStage(StageController controller) : base(controller)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("MouseStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("MouseStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        Debug.Log("MouseStage Destroy");
    }
}
