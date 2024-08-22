using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootingTutorial : MonoBehaviour
{
    PlayerActions actions;
    private Rigidbody2D rigid;
    [SerializeField] private GameObject projectilePrefab;
    private float upForce;
    private float shootTimer;
    private float shootDelay = 0.5f;

    private void Awake()
    {
        actions = new PlayerActions();
        rigid = GetComponent<Rigidbody2D>();

        actions.Shooting.Up.performed += Up;
        actions.Shooting.Up.canceled += Down;
    }

    private void OnEnable()
    {
        actions.Shooting.Enable();
        transform.position = new Vector2(-4.5f, 0.5f);
        transform.rotation = Quaternion.identity;
        rigid.velocity = Vector2.zero;
    }

    private void OnDisable()
    {
        actions.Shooting.Disable();
    }

    private void Update()
    {
        rigid.AddForce(Time.deltaTime * upForce * Vector2.up, ForceMode2D.Impulse);
        float zRotation = Mathf.Clamp(rigid.velocity.y * 10, -90, 90);
        transform.rotation = Quaternion.Euler(0, 0, zRotation);

        if (actions.Shooting.Shoot.ReadValue<float>() > 0.1f)
        {
            Shoot();
        }
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
            projectile.transform.position = transform.position;
            projectile.transform.rotation = Quaternion.identity;
            projectile.GetComponent<Projectile>().Launch(Vector2.right);
        }
    }
}
