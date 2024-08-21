using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStage
{
    public event Action<float> OnTimeUpdate;
    protected readonly StageController controller;
    private const float STAGE_TIME_MAX = 10f;
    private float stageTime;

    public BaseStage(StageController controller)
    {
        this.controller = controller;
        OnTimeUpdate += controller.SetTimeText;
        OnTimeUpdate += controller.TryEnableBulletItem;
    }

    public virtual void Initialize()
    {
        stageTime = 0f;
    }

    public virtual void Update()
    {
        stageTime += Time.deltaTime;
        OnTimeUpdate?.Invoke(stageTime);
    }

    public virtual void Destroy()
    {
        controller.DestroyMap();
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
