using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnShootingStage : PlayerOnStage
{
    private GameObject projectilePrefab;
    private float shootTimer;
    private float shootDelay = 0.5f;
    private float upForce;

    public PlayerOnShootingStage(Player player) : base(player) 
    {
        projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");

        player.Actions.Shooting.Up.performed += Up;
        player.Actions.Shooting.Up.canceled += Down;
    }

    public override void OnEnter()
    {
        player.Actions.Shooting.Enable();
        player.Rigid.gravityScale = 1;

        shootTimer = Time.time;
    }

    public override void OnUpdate()
    {
        player.Rigid.AddForce(Time.deltaTime * upForce * Vector2.up, ForceMode2D.Impulse);
        if (player.Actions.Shooting.Shoot.ReadValue<float>() > 0.1f)
        {
            Shoot();
        }
    }

    public override void OnExit()
    {
        player.Actions.Shooting.Disable();
        player.Rigid.gravityScale = 0;
        player.Rigid.velocity = Vector2.zero;
    }

    private void Up(InputAction.CallbackContext context)
    {
        upForce = 20f;
    }

    private void Down(InputAction.CallbackContext context)
    {
        upForce = 0f;
    }

    public void Shoot()
    {
        if (shootTimer < Time.time)
        {
            shootTimer = Time.time + shootDelay;

            GameObject projectile = Managers.ObjectPool.GetObject(projectilePrefab);
            projectile.transform.position = player.transform.position;
            projectile.GetComponent<Projectile>().Launch(Vector2.right);
        }
    }
}
