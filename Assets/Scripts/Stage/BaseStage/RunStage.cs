using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStage : BaseStage
{
    private List<GameObject> patterns = new();
    private IPattern pattern;

    public RunStage(StageController controller) : base(controller)
    {
        patterns.Add(Resources.Load<GameObject>("Prefabs/RunStage/Pattern_0"));
        patterns.Add(Resources.Load<GameObject>("Prefabs/RunStage/Pattern_1"));
        patterns.Add(Resources.Load<GameObject>("Prefabs/RunStage/Pattern_2"));
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("RunStage Initialize");
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
        base.Destroy();
        Debug.Log("RunStage Destroy");
        pattern?.Destroy();
    }
}
