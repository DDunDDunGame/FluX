using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnBossStage : PlayerOnStage
{
    private GameObject projectilePrefab;
    private Vector2 touchPosition;
    private Texture2D bossCurser;
    PhysicsMaterial2D jumpMaterial;
    private float playerHalfHeight;
    private bool isJumping;
    private int currentJumpCount;
    private int maxJumpCount = 2;
    private bool isGrounded;
    private float jumpForce = 10f;

    public PlayerOnBossStage(Player player) : base(player) {
        projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
        bossCurser = Resources.Load<Texture2D>("Arts/BossCursor");
        jumpMaterial = Resources.Load("Physics/Jump") as PhysicsMaterial2D;
    }
    public override void OnEnter()
    {
        player.Rigid.bodyType = RigidbodyType2D.Dynamic;
        Cursor.lockState = CursorLockMode.Confined;
        player.Rigid.gravityScale = 3;
        player.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.Coll.sharedMaterial = jumpMaterial;
        Cursor.SetCursor(bossCurser, Vector2.zero, CursorMode.Auto);

        player.Actions.Jump.Enable();
        player.Actions.Jump.Move.performed += Move;
        player.Actions.Jump.Move.canceled += Move;
        player.Actions.Jump.Jump.performed += Jump;
        player.Actions.Jump.Shoot.performed += Shoot;
        playerHalfHeight = player.Coll.bounds.extents.y;
        player.Sprite.sprite = player.Square;
    }

    public override void OnUpdate()
    {
        Vector2 mousePos = player.Actions.Jump.MousePos.ReadValue<Vector2>();
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);
        touchPosition = Camera.main.ScreenToWorldPoint(mousePos);

        isGrounded = IsGrounded();
        if (isGrounded) { currentJumpCount = 0; isJumping = false; }
        else if (!isJumping) currentJumpCount = 1;
    }

    public override void OnExit()
    {
        player.Actions.Jump.Shoot.performed -= Shoot;
        player.Actions.Jump.Disable();
        player.Rigid.gravityScale = 0;
        player.Rigid.constraints = RigidbodyConstraints2D.None;
        player.Coll.sharedMaterial = null;
        player.Rigid.velocity = Vector2.zero;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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

    private void Shoot(InputAction.CallbackContext context)
    {
        if (player.Stat.CanUsingBullet(1))
        {
            player.Stat.UseBullet(1);
            Vector2 bulletDir = (touchPosition - (Vector2)player.transform.position).normalized;
            GameObject projectile = Managers.ObjectPool.GetObject(projectilePrefab);
            projectile.transform.position = player.transform.position;
            projectile.transform.right = Vector3.Slerp(projectile.transform.right.normalized, bulletDir, 360);
            projectile.GetComponent<Projectile>().Launch(bulletDir);
        }
    }
}
