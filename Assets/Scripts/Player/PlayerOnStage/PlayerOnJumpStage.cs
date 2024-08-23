using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnJumpStage : PlayerOnStage
{
    bool isAir = false;
    int jumpCount = 2;
    PhysicsMaterial2D jumpMaterial;
    private float playerHalfHeight;

    public PlayerOnJumpStage(Player player) : base(player) 
    {
        jumpMaterial = Resources.Load("Physics/Jump") as PhysicsMaterial2D;
    }
    public override void OnEnter()
    {
        player.Rigid.bodyType = RigidbodyType2D.Dynamic;
        player.Rigid.gravityScale = 3f;
        player.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.Coll.sharedMaterial = jumpMaterial;
        player.Sprite.sprite = player.Square;

        player.Actions.Jump.Enable();
        player.Actions.Jump.Move.performed += Move;
        player.Actions.Jump.Move.canceled += Move;
        player.Actions.Jump.Jump.performed += Jump;
        playerHalfHeight = player.Coll.bounds.extents.y;
    }

    public override void OnUpdate()
    {
        // 떨어지고 있을 때
        if (isAir)
        {
            LandGround();
        }
    }

    public override void OnExit()
    {
        player.Actions.Jump.Disable();
        player.Rigid.gravityScale = 0;
        player.Rigid.constraints = RigidbodyConstraints2D.None;
        player.Coll.sharedMaterial = null;
        player.Rigid.velocity = new Vector2(0, 0);
        player.Coll.isTrigger = false;
        player.Rigid.velocity = Vector2.zero;
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        player.Rigid.velocity = new Vector2(moveInput.x * 5, player.Rigid.velocity.y);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!isAir || jumpCount > 0) 
        {
            player.Coll.isTrigger = true;
            player.Rigid.velocity = new Vector2(player.Rigid.velocity.x, 0);
            isAir = true;
            if (jumpCount == 2) SoundManager.Instance.PlaySound2D("SFX JumpOne");
            else SoundManager.Instance.PlaySound2D("SFX JumpTwo");
            jumpCount--;
            player.Rigid.AddForce(Vector3.up * 9, ForceMode2D.Impulse);
        }
    }

    private void LandGround()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(player.transform.position, Vector3.down, playerHalfHeight, LayerMask.GetMask("Ground"));
        if (hitGround)
        {
            player.Coll.isTrigger = false;
            player.Rigid.position = hitGround.point + new Vector2(0f, playerHalfHeight);
            isAir = false;
            jumpCount = 2;
        }
    }
}
