using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitStage : BaseStage
{
    GameObject mainCircle;
    GameObject player;
    public OrbitStage(StageController controller) : base(controller)
    {
        mainCircle = Resources.Load("Prefabs/Orbit/TempOrbit") as GameObject;
        player = GameObject.Find("Player");
    }

    public override void Initialize()
    {
        base.Initialize();
        mainCircle = controller.CreateMap(mainCircle, new Vector3(0, 0, 0));
        player.transform.position = mainCircle.transform.position + new Vector3(0, -2.5f, 0);
        Debug.Log("OrbitStage Initialize");
    }

    public override void Update()
    {
        base.Update();
        //Debug.Log("OrbitStage Update");
    }

    public override void Destroy()
    {
        base.Destroy();
        Debug.Log("OrbitStage Destroy");
    }
}
