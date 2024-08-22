using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnShootingStage : PlayerOnStage
{
    private GameObject projectilePrefab;
    private ParticleSystem shootingTrail;
    private float shootTimer;
    private float shootDelay = 0.5f;
    private float upForce;

    public PlayerOnShootingStage(Player player) : base(player) 
    {
        shootingTrail = Util.FindChild<ParticleSystem>(player.gameObject, "ShootingTrail");
        shootingTrail.gameObject.SetActive(false);
        projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
        player.Rigid.velocity = Vector2.zero;

        player.Actions.Shooting.Up.performed += Up;
        player.Actions.Shooting.Up.canceled += Down;
    }

    public override void OnEnter()
    {
        player.Sprite.sprite = player.ShootingSprite;
        player.Actions.Shooting.Enable();
        player.Rigid.gravityScale = 1;
        player.transform.position = new Vector3(-7.5f, 0, 0);
        shootingTrail.gameObject.SetActive(true);

        shootTimer = Time.time;
    }

    public override void OnUpdate()
    {
        player.Rigid.AddForce(Time.deltaTime * upForce * Vector2.up, ForceMode2D.Impulse);
        float zRotation = Mathf.Clamp(player.Rigid.velocity.y * 10, -90, 90);
        player.transform.rotation = Quaternion.Euler(0, 0, zRotation);

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
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        shootingTrail.Stop();
        shootingTrail.gameObject.SetActive(false);
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
            projectile.transform.rotation = Quaternion.identity;
            projectile.GetComponent<Projectile>().Launch(Vector2.right);
        }
    }
}
