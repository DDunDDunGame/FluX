using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnJumpStage : PlayerOnStage
{
    public PlayerOnJumpStage(Player player) : base(player) { }
    public override void OnEnter()
    {

    }

    public override void OnJump(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            player.Rigid.AddForce(Vector3.up * 4, ForceMode2D.Impulse);
        }
    }

    public override void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        player.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (input != null)
        {
            player.Rigid.velocity = new Vector3(input.x, 0, input.y) * 5;
        }
    }

    public override void OnShoot(InputAction.CallbackContext context)
    {

    }
}
