using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStage : BaseStage
{
    GameObject tiles;
    public JumpStage(StageController controller) : base(controller)
    {
        tiles = Resources.Load("Prefabs/JumpStage/Maps") as GameObject;
    }

    public override void Initialize()
    {
        base.Initialize();
        tiles = Util.MapCreate(tiles, new Vector3(0, 0, 0));
        GameObject.Find("Player").transform.position = tiles.transform.position + new Vector3(0, -3, 0);
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
        Util.MapDestroy();
        Debug.Log("JumpStage Destroy");
    }
}
