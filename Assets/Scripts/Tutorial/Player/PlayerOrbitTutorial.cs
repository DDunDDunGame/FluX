using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerOrbitTutorial : MonoBehaviour
{
    private Vector3 orbitCenter = new Vector3(0, -0.52f, 0);
    private PlayerActions actions;
    private Rigidbody2D rigid;
    private float inputX;

    private void Awake()
    {
        actions = new PlayerActions();
        rigid = GetComponent<Rigidbody2D>();
        actions.Orbit.Move.performed += Move;
        actions.Orbit.Move.canceled += Move;
    }

    private void OnEnable()
    {
        actions.Orbit.Enable();
        transform.SetPositionAndRotation(new Vector2(0f, -1.5f), Quaternion.identity);
        inputX = 0f;
    }

    private void Update()
    {
        transform.RotateAround(orbitCenter, Vector3.forward, -inputX * 300 * Time.deltaTime);
    }

    private void OnDisable()
    {
        actions.Orbit.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputX = input.x;
    }
}
