using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnMouseStage : PlayerOnStage
{
    private GameObject mouseTrail;
    public PlayerOnMouseStage(Player player) : base(player) 
    {
        mouseTrail = Util.FindChild(player.gameObject, "MouseTrail");
        mouseTrail.SetActive(false);
    }
    public override void OnEnter() 
    {
        player.Actions.Mouse.Enable();
        Cursor.lockState = CursorLockMode.Confined;
        player.transform.position = Vector3.zero;
        player.Sprite.sprite = player.CircleIdle;
        player.Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        mouseTrail.SetActive(true);
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
        player.Rigid.constraints = RigidbodyConstraints2D.None;
        mouseTrail.SetActive(false);
    }
}
