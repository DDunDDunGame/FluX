using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStage : BaseStage
{
    public ShootingStage(StageController controller) : base(controller)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("ShootingStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("ShootingStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        Debug.Log("ShootingStage Destroy");
    }
}
