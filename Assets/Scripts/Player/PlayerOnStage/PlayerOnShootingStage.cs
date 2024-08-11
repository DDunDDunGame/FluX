using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnShootingStage : PlayerOnStage
{
    private GameObject projectilePrefab;
    private float shootTimer;
    private float shootDelay = 0.5f;
    private float moveSpeed = 5f;

    public PlayerOnShootingStage(Player player) : base(player) 
    {
        projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
    }

    public override void OnEnter()
    {
        player.Rigid.gravityScale = 0;
        shootTimer = Time.time;
    }

    public override void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        player.Rigid.velocity = value * moveSpeed;
    }

    public override void OnShoot(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(shootTimer < Time.time)
            {
                shootTimer = Time.time + shootDelay;

                GameObject projectile = Managers.ObjectPool.GetObject(projectilePrefab);
                projectile.transform.position = player.transform.position;
                projectile.GetComponent<Projectile>().Launch(Vector2.right);
            }
        }
    }

    public override void OnJump(InputAction.CallbackContext context)
    {
        return;
    }
}
