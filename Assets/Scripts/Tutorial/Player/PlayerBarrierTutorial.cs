using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerBarrierTutorial : MonoBehaviour
{
    private PlayerActions actions;
    private Rigidbody2D rigid;

    private void Awake()
    {
        actions = new PlayerActions();
        rigid = GetComponent<Rigidbody2D>();
        actions.Barrier.Move.performed += Move;
        actions.Barrier.Move.canceled += Move;
    }

    private void OnEnable()
    {
        actions.Barrier.Enable();
        transform.SetPositionAndRotation(new Vector2(0f, -0.5f), Quaternion.identity);
        rigid.angularVelocity = 0f;
    }

    private void OnDisable()
    {
        actions.Barrier.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        rigid.angularVelocity = input.x * 300;
    }
}
