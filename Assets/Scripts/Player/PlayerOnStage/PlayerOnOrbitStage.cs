using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerOnOrbitStage : PlayerOnStage
{
    public PlayerOnOrbitStage(Player player) : base(player) 
    {
        player.Actions.Orbit.Move.performed += Move;
        player.Actions.Orbit.Move.canceled += Move;
    }
    public override void OnEnter()
    {
        player.Actions.Orbit.Enable();
    }

    public override void OnUpdate()
    {
        Debug.Log("Player Update");
    }

    public override void OnExit()
    {
        player.Actions.Orbit.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        player.transform.parent.GetComponent<Rigidbody2D>().angularVelocity = input.x * 300;
    }
}
