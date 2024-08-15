using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStage
{
    private readonly StageController controller;
    private const float STAGE_TIME_MAX = 10f;
    private float stageTime;

    public BaseStage(StageController controller)
    {
        this.controller = controller;
    }

    public virtual void Initialize()
    {
        stageTime = 0f;
    }

    public virtual void Update()
    {
        stageTime += Time.deltaTime;
    }

    public virtual void Destroy()
    {

    }

    public virtual bool IsEnd()
    {
        if(stageTime >= STAGE_TIME_MAX)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
