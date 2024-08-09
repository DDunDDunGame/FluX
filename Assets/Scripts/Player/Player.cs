using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IStageAttachment
{
    private Define.Stage currentStage;
    private Rigidbody2D rigid;
    private PlayerInput input;

    private void Awake()
    {
        InitVariables();
        input.SwitchCurrentActionMap(Define.Stage.None.ToString());
    }

    private void InitVariables()
    {
        currentStage = Define.Stage.None;
        rigid = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
    }

    public void ChangeStage(Define.Stage stage)
    {
        currentStage = stage;
        input.SwitchCurrentActionMap(stage.ToString());
    }

    // 추후에 활성화된 스테이지 별로 다른 함수가 작동하게 설계
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        rigid.velocity = input;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Shoot");
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump");
        }
    }
}
