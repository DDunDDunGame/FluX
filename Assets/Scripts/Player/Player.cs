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

    // ���Ŀ� Ȱ��ȭ�� �������� ���� �ٸ� �Լ��� �۵��ϰ� ����
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
