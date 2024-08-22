using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnJumpStage : PlayerOnStage
{
    bool isAir = false;
    int jumpCount = 2;
    Sprite playerSprite;

    public PlayerOnJumpStage(Player player) : base(player) 
    {
        playerSprite = Resources.Load<Sprite>("Arts/Player/SquarePlayer");
    }
    public override void OnEnter()
    {
        player.Rigid.bodyType = RigidbodyType2D.Dynamic;
        player.Rigid.gravityScale = 2;
        player.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.transform.GetComponent<BoxCollider2D>().sharedMaterial = Resources.Load("Physics/Jump") as PhysicsMaterial2D;
        player.Sprite.sprite = playerSprite;

        player.Actions.Jump.Enable();
        player.Actions.Jump.Move.performed += Move;
        player.Actions.Jump.Move.canceled += Move;
        player.Actions.Jump.Jump.performed += Jump;
    }

    public override void OnUpdate()
    {
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
        player.transform.GetComponent<CircleCollider2D>().sharedMaterial = null;
        player.Rigid.velocity = new Vector2(0, 0);
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        player.Rigid.velocity = new Vector2(moveInput.x * 5, player.Rigid.velocity.y);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!isAir && jumpCount > 0) 
        {
            isAir = true;
            if (jumpCount == 2) SoundManager.Instance.PlaySound2D("SFX JumpOne");
            else SoundManager.Instance.PlaySound2D("SFX JumpTwo");
            jumpCount--;
            float JumpInput = player.Actions.Jump.Jump.ReadValue<float>();
            player.Rigid.AddForce(Vector3.up * 6, ForceMode2D.Impulse);
        }
    }

    private void LandGround()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(player.transform.position, Vector3.down, 0.55f, LayerMask.GetMask("Ground"));
        if (hitGround)
        {
            isAir = false;
            jumpCount = 2;
        }
    }
}
