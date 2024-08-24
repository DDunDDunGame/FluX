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
    private bool isJumping;
    private LayerMask platformMask;
    private bool isGrounded;
    private ParticleSystem landEffect;

    private void Awake()
    {
        actions = new PlayerActions();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        landEffect = Util.FindChild<ParticleSystem>(gameObject, "LandEffect");
        playerHalfHeight = coll.bounds.extents.y;
        platformMask = LayerMask.GetMask("Ground");

        actions.Run.Jump.performed += Jump;
        actions.Run.Down.performed += Down;
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        if (isGrounded) { currentJumpCount = 0; isJumping = false; }
        else if (!isJumping) currentJumpCount = 1;
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
        if (currentJumpCount >= maxJumpCount) return;

        isJumping = true;
        currentJumpCount++;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (currentJumpCount == 1) { SoundManager.Instance.PlaySound2D("SFX JumpOne"); }
        else { SoundManager.Instance.PlaySound2D("SFX JumpTwo"); }
    }

    private void Down(InputAction.CallbackContext context)
    {
        if (isGrounded) return;
        Vector2 origin = coll.bounds.center - new Vector3(0, playerHalfHeight);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 15f, platformMask);
        if (hit)
        {
            rigid.position = hit.point + new Vector2(0f, playerHalfHeight);
            rigid.velocity = Vector2.zero;
        }
        SoundManager.Instance.PlaySound2D("SFX Landing");
        landEffect.Play();
    }

    private bool IsGrounded()
    {
        Vector2 origin = coll.bounds.center - new Vector3(0, playerHalfHeight);
        float boxWidth = coll.bounds.size.x;
        Vector2 size = new(boxWidth + 0.2f, 0.1f);
        RaycastHit2D centerHit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0f, platformMask);

        return centerHit.normal == Vector2.up;
    }
}
