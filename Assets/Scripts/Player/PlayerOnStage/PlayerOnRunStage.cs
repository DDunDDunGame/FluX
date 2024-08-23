using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerOnRunStage : PlayerOnStage
{
    private float jumpForce = 10f;
    private float playerHalfHeight;
    private int maxJumpCount = 2;
    private int currentJumpCount = 0;
    private Vector2 initPos = new(-7f, -2.5f);
    private LayerMask platformMask = LayerMask.GetMask("Ground");

    public PlayerOnRunStage(Player player) : base(player) 
    {
        player.Actions.Run.Jump.performed += Jump;
        player.Actions.Run.Down.performed += Down;
    }
    public override void OnEnter()
    {
        player.Actions.Run.Enable();
        player.transform.position = initPos;
        player.Rigid.bodyType = RigidbodyType2D.Dynamic;
        player.Rigid.gravityScale = 3;
        player.Rigid.constraints = (int)RigidbodyConstraints2D.FreezePositionX + RigidbodyConstraints2D.FreezeRotation;
        player.Sprite.sprite = player.Square;
        currentJumpCount = maxJumpCount;
        playerHalfHeight = player.Coll.bounds.extents.y;
        player.OnBottomHit -= DownOnTop;
        player.OnBottomHit += DownOnTop;
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
        player.OnBottomHit -= DownOnTop;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded()) currentJumpCount = 0;
        if (currentJumpCount >= maxJumpCount) return;
        SoundManager.Instance.PlaySound2D("SFX JumpOne");
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
        SoundManager.Instance.PlaySound2D("SFX Landing");
        player.LandEffect.Play();
    }

    private bool IsGrounded()
    {
        Vector2 origin = player.Coll.bounds.center - new Vector3(0, playerHalfHeight);
        float boxWidth = player.Coll.bounds.size.x;
        Vector2 size = new(boxWidth, 0.05f);
        RaycastHit2D centerHit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0.1f, platformMask);

        return centerHit.normal == Vector2.up;
    }

    private void DownOnTop()
    {
        player.transform.position = new Vector2(initPos.x, 4.8f);
        player.Rigid.velocity = Vector2.zero;
    }
}
