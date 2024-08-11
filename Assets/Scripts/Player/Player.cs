using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IStageAttachment
{
    private Define.Stage currentStage;
    public Rigidbody2D Rigid { get; private set; }
    private PlayerInput input;
    private Dictionary<Define.Stage, PlayerOnStage> onStageDict;

    private void Awake()
    {
        InitVariables();
        input.SwitchCurrentActionMap(Define.Stage.None.ToString());
    }

    private void InitVariables()
    {
        currentStage = Define.Stage.None;
        Rigid = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
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
        currentStage = stage;
        input.SwitchCurrentActionMap(stage.ToString());
        onStageDict[currentStage].OnEnter();
    }

    // ���Ŀ� Ȱ��ȭ�� �������� ���� �ٸ� �Լ��� �۵��ϰ� ����
    public void OnMove(InputAction.CallbackContext context)
    {
        onStageDict[currentStage].OnMove(context);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        onStageDict[currentStage].OnShoot(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        onStageDict[currentStage].OnJump(context);
    }
}
