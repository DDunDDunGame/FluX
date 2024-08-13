using UnityEngine.InputSystem;

public abstract class PlayerOnStage
{
    protected Player player;
    public PlayerOnStage(Player player)
    {
        this.player = player;
    }
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}
