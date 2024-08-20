using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnRunStage : PlayerOnStage
{
    private float jumpForce = 10f;
    private float playerHalfHeight;
    private int maxJumpCount = 2;
    private int currentJumpCount = 0;
    private LayerMask platformMask = LayerMask.GetMask("Ground");

    public PlayerOnRunStage(Player player) : base(player) 
    {
        player.Actions.Run.Jump.performed += Jump;
        player.Actions.Run.Down.performed += Down;
    }
    public override void OnEnter()
    {
        player.Actions.Run.Enable();
        player.Rigid.gravityScale = 3;
        player.Rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
        currentJumpCount = maxJumpCount;
        playerHalfHeight = player.Coll.bounds.extents.y;
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
        player.Actions.Run.Disable();
        player.Rigid.gravityScale = 0;
        player.Rigid.velocity = Vector2.zero;
        player.Rigid.constraints = RigidbodyConstraints2D.None;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(IsGrounded()) currentJumpCount = 0;
        if(currentJumpCount >= maxJumpCount) return;

        currentJumpCount++;
        player.Rigid.velocity = Vector2.zero;
        player.Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Down(InputAction.CallbackContext context)
    {
        if (IsGrounded()) return;
        Vector2 origin = player.Coll.bounds.center - new Vector3(0, playerHalfHeight);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 15f, platformMask);
        if (hit)
        {
            player.Rigid.position = hit.point + new Vector2(0f, playerHalfHeight);
            player.Rigid.velocity = Vector2.zero;
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = player.Coll.bounds.center - new Vector3(0, playerHalfHeight);
        Vector2 size = new(player.Coll.bounds.size.x, 0.1f);
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0.1f, platformMask);

        return hit;
    }
}
