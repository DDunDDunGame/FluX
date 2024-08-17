using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnBarrierStage : PlayerOnStage
{
    public PlayerOnBarrierStage(Player player) : base(player) 
    {

    }
    public override void OnEnter()
    {
        player.Rigid.angularDrag = 0;
        player.Rigid.bodyType = RigidbodyType2D.Kinematic;

        player.Actions.Barrier.Enable();
        player.Actions.Barrier.Move.performed += Move;
        player.Actions.Barrier.Move.canceled += Move;
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        player.Actions.Barrier.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        player.Rigid.angularVelocity = input.x * 300;
    }
}
