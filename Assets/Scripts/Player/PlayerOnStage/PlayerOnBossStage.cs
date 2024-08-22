using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnBossStage : PlayerOnStage
{
    bool isAir = false;
    int jumpCount = 2;
    Sprite playerSprite;
    private GameObject projectilePrefab;
    private Vector2 touchPosition;

    public PlayerOnBossStage(Player player) : base(player) {
        playerSprite = Resources.Load<Sprite>("Arts/Player/SquarePlayer");
        projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
    }
    public override void OnEnter()
    {
        player.Rigid.bodyType = RigidbodyType2D.Dynamic;
        Cursor.lockState = CursorLockMode.Confined;
        player.Rigid.gravityScale = 2;
        player.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.Coll.sharedMaterial = Resources.Load("Physics/Jump") as PhysicsMaterial2D;

        player.Actions.Jump.Enable();
        player.Actions.Jump.Move.performed += Move;
        player.Actions.Jump.Move.canceled += Move;
        player.Actions.Jump.Jump.performed += Jump;
        player.Actions.Jump.Shoot.performed += Shoot;
        player.Sprite.sprite = playerSprite;
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
        player.Actions.Jump.Disable();
        player.Rigid.gravityScale = 0;
        player.Rigid.constraints = RigidbodyConstraints2D.None;
        player.Coll.sharedMaterial = null;
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
            jumpCount--;
            float JumpInput = player.Actions.Jump.Jump.ReadValue<float>();
            player.Rigid.AddForce(Vector3.up * 6, ForceMode2D.Impulse);
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
        RaycastHit2D hitGround = Physics2D.Raycast(player.transform.position, Vector3.down, 0.55f, LayerMask.GetMask("Ground"));
        if (hitGround)
        {
            isAir = false;
            jumpCount = 2;
        }
    }
}
