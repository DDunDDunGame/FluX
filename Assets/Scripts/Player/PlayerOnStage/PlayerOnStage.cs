using UnityEngine.InputSystem;

public abstract class PlayerOnStage
{
    protected Player player;
    public PlayerOnStage(Player player)
    {
        this.player = player;
    }
    public abstract void OnEnter();
    public abstract void OnMove(InputAction.CallbackContext context);
    public abstract void OnShoot(InputAction.CallbackContext context);
    public abstract void OnJump(InputAction.CallbackContext context);
}
