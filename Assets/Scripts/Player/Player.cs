using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IStageAttachment
{
    private Define.Stage currentStage;
    public Rigidbody2D Rigid { get; private set; }
    public Collider2D Coll { get; private set; }
    public PlayerActions Actions { get; private set; }
    private Dictionary<Define.Stage, PlayerOnStage> onStageDict;

    private void Awake()
    {
        InitVariables();
    }

    private void Update()
    {
        if(currentStage != Define.Stage.None)
        {
            onStageDict[currentStage].OnUpdate();
        }
    }

    private void InitVariables()
    {
        currentStage = Define.Stage.None;
        Rigid = GetComponent<Rigidbody2D>();
        Coll = GetComponent<Collider2D>();
        Actions = new PlayerActions();
        onStageDict = new Dictionary<Define.Stage, PlayerOnStage>
        {
            { Define.Stage.Shooting, new PlayerOnShootingStage(this) },
            { Define.Stage.Mouse, new PlayerOnMouseStage(this) },
            { Define.Stage.Run, new PlayerOnRunStage(this) },
            { Define.Stage.Jump, new PlayerOnJumpStage(this) },
            { Define.Stage.Orbit, new PlayerOnOrbitStage(this) },
            { Define.Stage.Barrier, new PlayerOnBarrierStage(this) },
            { Define.Stage.Boss, new PlayerOnBossStage(this) }
        };
    }

    public void ChangeStage(Define.Stage stage)
    {
        if(currentStage != Define.Stage.None)
        {
            onStageDict[currentStage].OnExit();
        }
        currentStage = stage;
        onStageDict[currentStage].OnEnter();
    }
}
