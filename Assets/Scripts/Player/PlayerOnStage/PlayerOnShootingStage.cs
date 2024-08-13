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

        player.Actions.Shooting.Move.performed += Move;
        player.Actions.Shooting.Move.canceled += Move;
    }

    public override void OnEnter()
    {
        player.Actions.Shooting.Enable();
        player.Rigid.gravityScale = 0;
        shootTimer = Time.time;
    }

    public override void OnUpdate()
    {
        if (player.Actions.Shooting.Shoot.ReadValue<float>() > 0.1f)
        {
            Shoot();
        }
    }

    public override void OnExit()
    {
        player.Actions.Shooting.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        Debug.Log(value);
        player.Rigid.velocity = value * moveSpeed;
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
