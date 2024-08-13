using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnMouseStage : PlayerOnStage
{
    public PlayerOnMouseStage(Player player) : base(player) { }
    public override void OnEnter() 
    {
        player.Actions.Mouse.Enable();
        Cursor.lockState = CursorLockMode.Confined;
    }

    public override void OnUpdate()
    {
        Vector2 mousePos = player.Actions.Mouse.Move.ReadValue<Vector2>();
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        player.Rigid.position = worldPos;
    }

    public override void OnExit()
    {
        player.Actions.Mouse.Disable();
    }
}
