using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStage : BaseStage
{
    public JumpStage(StageController controller) : base(controller)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("JumpStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("JumpStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        Debug.Log("JumpStage Destroy");
    }
}
