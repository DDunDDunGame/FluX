using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStage : BaseStage
{
    private List<GameObject> patterns = new();
    private IPattern pattern;

    public MouseStage(StageController controller) : base(controller)
    {
        //patterns.Add(Resources.Load<GameObject>("Prefabs/MouseStage/Pattern_0"));
        //patterns.Add(Resources.Load<GameObject>("Prefabs/MouseStage/Pattern_1"));
        patterns.Add(Resources.Load<GameObject>("Prefabs/MouseStage/Pattern_2"));
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("MouseStage Initialize");
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
        Debug.Log("MouseStage Destroy");
        pattern?.Destroy();
    }
}
