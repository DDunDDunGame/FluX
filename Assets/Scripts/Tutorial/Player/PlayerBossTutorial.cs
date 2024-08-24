using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBossTutorial : MonoBehaviour
{
    private PlayerActions actions;
    private PhysicsMaterial2D jumpMaterial;
    private Collider2D coll;
    private Rigidbody2D rigid;
    [SerializeField] private GameObject projectilePrefab;
    private float playerHalfHeight;
    private Vector2 touchPosition;
    private bool isJumping;
    private int currentJumpCount;
    private int maxJumpCount = 2;
    private bool isGrounded;
    private float jumpForce = 8f;

    private void Awake()
    {
        actions = new PlayerActions();
        jumpMaterial = Resources.Load<PhysicsMaterial2D>("Physics/Jump");
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        coll.sharedMaterial = jumpMaterial;
        actions.Jump.Move.performed += Move;
        actions.Jump.Move.canceled += Move;
        actions.Jump.Jump.performed += Jump;
        playerHalfHeight = coll.bounds.extents.y;
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

        isGrounded = IsGrounded();
        if (isGrounded) { currentJumpCount = 0; isJumping = false; }
        else if (!isJumping) currentJumpCount = 1;
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
        if (currentJumpCount >= maxJumpCount) return;

        isJumping = true;
        currentJumpCount++;
        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (currentJumpCount == 1) { SoundManager.Instance.PlaySound2D("SFX JumpOne"); }
        else { SoundManager.Instance.PlaySound2D("SFX JumpTwo"); }
    }

    private bool IsGrounded()
    {
        Vector2 origin = coll.bounds.center - new Vector3(0, playerHalfHeight);
        float boxWidth = coll.bounds.size.x;
        Vector2 size = new(boxWidth + 0.2f, 0.1f);
        RaycastHit2D centerHit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0f, LayerMask.GetMask("Ground"));

        return centerHit.normal == Vector2.up;
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
