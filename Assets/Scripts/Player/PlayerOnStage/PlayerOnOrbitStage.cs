using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnOrbitStage : PlayerOnStage
{
    public PlayerOnOrbitStage(Player player) : base(player) { }
    public override void OnEnter()
    {

    }

    public override void OnJump(InputAction.CallbackContext context)
    {

    }

    public override void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        player.Rigid.bodyType = RigidbodyType2D.Kinematic;
        if (input != null)
        {
            player.transform.parent.GetComponent<Rigidbody2D>().angularVelocity = input.x * 300;
        }
    }

    public override void OnShoot(InputAction.CallbackContext context)
    {

    }
}
