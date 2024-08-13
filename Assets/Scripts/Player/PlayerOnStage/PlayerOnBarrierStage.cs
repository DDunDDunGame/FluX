using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerOnBarrierStage : PlayerOnStage
{
    public PlayerOnBarrierStage(Player player) : base(player) { }
    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {

    }

    public override void OnMove(InputAction.CallbackContext context)
    {
        player.Rigid.angularDrag = 0;
        Vector2 input = context.ReadValue<Vector2>();

        if (input != null)
        {
            player.Rigid.angularVelocity = input.x * 300;
        }
    }

    public override void OnShoot(InputAction.CallbackContext context)
    public override void OnExit()
    {

    }
}
