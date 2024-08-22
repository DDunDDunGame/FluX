using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpTutorial : MonoBehaviour
{
    private PlayerActions actions;
    private PhysicsMaterial2D jumpMaterial;
    private Rigidbody2D rigid;
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
    }

    private void Update()
    {
        if (isAir)
        {
            LandGround();
        }
    }

    private void OnDisable()
    {
        actions.Jump.Disable();
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
}
