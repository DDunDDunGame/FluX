using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnJumpStage : PlayerOnStage
{
    PhysicsMaterial2D jumpMaterial;
    private float playerHalfHeight;
    private bool isJumping;
    private int currentJumpCount;
    private int maxJumpCount = 2;
    private bool isGrounded;
    private float jumpForce = 10f;

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


        player.OnBottomHit -= DownOnTop;
        player.OnBottomHit += DownOnTop;
    }

    public override void OnUpdate()
    {
        isGrounded = IsGrounded();
        if (isGrounded) { currentJumpCount = 0; isJumping = false; }
        else if (!isJumping) currentJumpCount = 1;
    }

    public override void OnExit()
    {
        player.Actions.Jump.Disable();
        player.Rigid.gravityScale = 0;
        player.Rigid.constraints = RigidbodyConstraints2D.None;
        player.Coll.sharedMaterial = null;
        player.Rigid.velocity = Vector2.zero;
        player.OnBottomHit -= DownOnTop;
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        player.Rigid.velocity = new Vector2(moveInput.x * 5, player.Rigid.velocity.y);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (currentJumpCount >= maxJumpCount) return;

        isJumping = true;
        currentJumpCount++;
        player.Rigid.velocity = new Vector2(player.Rigid.velocity.x, 0);
        player.Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (currentJumpCount == 1) { SoundManager.Instance.PlaySound2D("SFX JumpOne"); }
        else { SoundManager.Instance.PlaySound2D("SFX JumpTwo"); }
    }

    private bool IsGrounded()
    {
        Vector2 origin = player.Coll.bounds.center - new Vector3(0, playerHalfHeight);
        float boxWidth = player.Coll.bounds.size.x;
        Vector2 size = new(boxWidth + 0.2f, 0.1f);
        RaycastHit2D centerHit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0f, LayerMask.GetMask("Ground"));

        return centerHit.normal == Vector2.up;
    }

    private void DownOnTop()
    {
        player.transform.position = new Vector2(0f, 4.8f);
        player.Rigid.velocity = new Vector2(player.Rigid.velocity.x, 0);
    }
}
