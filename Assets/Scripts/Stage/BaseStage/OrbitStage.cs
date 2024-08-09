using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitStage : BaseStage
{
    public OrbitStage(StageController controller) : base(controller)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("OrbitStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("OrbitStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        Debug.Log("OrbitStage Destroy");
    }
}
