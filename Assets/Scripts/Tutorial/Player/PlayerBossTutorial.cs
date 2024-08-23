using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBossTutorial : MonoBehaviour
{
    private PlayerActions actions;
    private PhysicsMaterial2D jumpMaterial;
    private Rigidbody2D rigid;
    [SerializeField] private GameObject projectilePrefab;
    private Vector2 touchPosition;
    bool isAir = false;
    int jumpCount = 2;

    private void Awake()
    {
        actions = new PlayerActions();
        jumpMaterial = Resources.Load<PhysicsMaterial2D>("Physics/Jump");
        rigid = GetComponent<Rigidbody2D>();
        GetComponent<Collider2D>().sharedMaterial = jumpMaterial;
        actions.Jump.Move.performed += Move;
        actions.Jump.Move.canceled += Move;
        actions.Jump.Jump.performed += Jump;
    }

    private void OnEnable()
    {
        actions.Jump.Enable();
        transform.SetPositionAndRotation(new Vector2(-3f, 0f), Quaternion.identity);
        rigid.velocity = Vector2.zero;
        actions.Jump.Shoot.performed += Shoot;
    }

    private void Update()
    {
        Vector2 mousePos = actions.Jump.MousePos.ReadValue<Vector2>();
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);
        touchPosition = Camera.main.ScreenToWorldPoint(mousePos);

        if (isAir)
        {
            LandGround();
        }
    }

    private void OnDisable()
    {
        actions.Jump.Disable();
        actions.Jump.Shoot.performed -= Shoot;
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        rigid.velocity = new Vector2(moveInput.x * 5, rigid.velocity.y);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!isAir && jumpCount > 0)
        {
            isAir = true;
            jumpCount--;
            float JumpInput = actions.Jump.Jump.ReadValue<float>();
            rigid.AddForce(Vector3.up * 6, ForceMode2D.Impulse);
        }
    }

    private void LandGround()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector3.down, 0.55f, LayerMask.GetMask("Ground"));
        if (hitGround)
        {
            isAir = false;
            jumpCount = 2;
        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Vector2 bulletDir = (touchPosition - (Vector2)transform.position).normalized;
        GameObject projectile = Managers.ObjectPool.GetObject(projectilePrefab);
        projectile.transform.position = transform.position;
        projectile.transform.right = Vector3.Slerp(projectile.transform.right.normalized, bulletDir, 360);
        projectile.GetComponent<Projectile>().Launch(bulletDir);
    }
}
