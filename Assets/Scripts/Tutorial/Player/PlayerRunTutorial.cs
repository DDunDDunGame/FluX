using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunTutorial : MonoBehaviour
{
    PlayerActions actions;
    private Rigidbody2D rigid;
    private Collider2D coll;
    [SerializeField] private float minY;

    private float jumpForce = 9f;
    private float playerHalfHeight;
    private int maxJumpCount = 2;
    private int currentJumpCount = 0;
    private LayerMask platformMask;

    private void Awake()
    {
        actions = new PlayerActions();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        playerHalfHeight = coll.bounds.extents.y;
        platformMask = LayerMask.GetMask("Ground");

        actions.Run.Jump.performed += Jump;
        actions.Run.Down.performed += Down;
    }

    private void OnEnable()
    {
        actions.Run.Enable();
        transform.position = new Vector2(-4.5f, 0.5f);
        transform.rotation = Quaternion.identity;
        rigid.velocity = Vector2.zero;
    }

    private void OnDisable()
    {
        actions.Run.Disable();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded()) currentJumpCount = 0;
        if (currentJumpCount >= maxJumpCount) return;

        currentJumpCount++;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Down(InputAction.CallbackContext context)
    {
        if (IsGrounded()) return;
        Vector2 origin = coll.bounds.center - new Vector3(0, playerHalfHeight);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 15f, platformMask);
        if (hit)
        {
            rigid.position = hit.point + new Vector2(0f, playerHalfHeight);
            rigid.velocity = Vector2.zero;
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = coll.bounds.center - new Vector3(0, playerHalfHeight);
        Vector2 size = new(coll.bounds.size.x, 0.1f);
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0.1f, platformMask);

        return hit;
    }
}
