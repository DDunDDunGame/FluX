using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BarrierStage : BaseStage
{
    GameObject barrier;
    GameObject player;
    public BarrierStage(StageController controller) : base(controller)
    {
        barrier = Resources.Load("Prefabs/BarrierStage/Barrier") as GameObject;
        player = GameObject.Find("Player");
    }

    public override void Initialize()
    {
        base.Initialize();
        barrier = Util.CreateObjToParent(barrier, new Vector3(0, 1.25f, 0), player);
        player.transform.position = new Vector3(0, 0, 0);
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
        Util.DestoryObjFromParent(player);
        Debug.Log("BarrierStage Destroy");
    }
}
