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
        player.Rigid.angularDrag = 0;
        player.Rigid.bodyType = RigidbodyType2D.Kinematic;
        player.Actions.Barrier.Enable();
    }

    public override void OnUpdate()
    {
        Vector2 input = player.Actions.Barrier.Move.ReadValue<Vector2>();
        if (input != null)
        {
            player.Rigid.angularVelocity = input.x * 300;
        }
    }

    public override void OnExit()
    {
        player.Actions.Barrier.Disable();
    }
}
