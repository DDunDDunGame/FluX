using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStage : BaseStage
{
    private List<GameObject> patterns = new List<GameObject>();
    private IPattern pattern;
    public ShootingStage(StageController controller) : base(controller)
    {
        //patterns.Add(Resources.Load<GameObject>("Prefabs/ShootingStage/Pattern_0"));
        patterns.Add(Resources.Load<GameObject>("Prefabs/ShootingStage/Pattern_1"));
        //patterns.Add(Resources.Load<GameObject>("Prefabs/ShootingStage/Pattern_2"));
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("ShootingStage Initialize");
        SetRandomPattern();
    }

    private void SetRandomPattern()
    {
        int randomIndex = Random.Range(0, patterns.Count);
        GameObject patternObject = controller.CreateMap(patterns[randomIndex], Vector3.zero);
        if (patternObject.TryGetComponent(out pattern)) { pattern.Initialize(controller.Player); };
    }

    public override void Update()
    {
        base.Update();
        pattern?.OnUpdate();
    }

    public override void Destroy()
    {
        pattern?.Destroy();
        base.Destroy();
        Debug.Log("ShootingStage Destroy");
    }
}
