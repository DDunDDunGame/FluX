using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnBossStage : PlayerOnStage
{
    bool isAir = false;
    int jumpCount = 2;
    private GameObject projectilePrefab;
    private Vector2 touchPosition;
    private float playerHalfHeight;

    public PlayerOnBossStage(Player player) : base(player) {
        projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
    }
    public override void OnEnter()
    {
        player.Rigid.bodyType = RigidbodyType2D.Dynamic;
        Cursor.lockState = CursorLockMode.Confined;
        player.Rigid.gravityScale = 3;
        player.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.Coll.sharedMaterial = Resources.Load("Physics/Jump") as PhysicsMaterial2D;

        player.Actions.Jump.Enable();
        player.Actions.Jump.Move.performed += Move;
        player.Actions.Jump.Move.canceled += Move;
        player.Actions.Jump.Jump.performed += Jump;
        player.Actions.Jump.Shoot.performed += Shoot;
        playerHalfHeight = player.Coll.bounds.extents.y;
    }

    public override void OnUpdate()
    {
        Vector2 mousePos = player.Actions.Jump.MousePos.ReadValue<Vector2>();
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);
        touchPosition = Camera.main.ScreenToWorldPoint(mousePos);

        if (isAir)
        {
            LandGround();
        }
    }

    public override void OnExit()
    {
        player.Actions.Jump.Shoot.performed -= Shoot;
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
