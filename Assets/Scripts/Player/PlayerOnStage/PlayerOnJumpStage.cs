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
            player.Rigid.AddForce(Vector3.up * 3, ForceMode2D.Impulse);
        }
    }

    public override void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // 이니셜라이져 부분으로 바꾸면 좋을것같아요
        player.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.transform.GetComponent<CircleCollider2D>().sharedMaterial = Resources.Load("Physics/Jump") as PhysicsMaterial2D;

        if (input != null)
        {
            player.Rigid.velocity = new Vector2(input.x * 5, player.Rigid.velocity.y);
        }
    }

    public override void OnShoot(InputAction.CallbackContext context)
    {

    }
}
