using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerOnOrbitStage : PlayerOnStage
{
    Sprite playerSprite;
    float inputX;

    public PlayerOnOrbitStage(Player player) : base(player) 
    {
        playerSprite = Resources.Load<Sprite>("Arts/Orbit/TrPlayer");
    }
    public override void OnEnter()
    {
        player.Rigid.bodyType = RigidbodyType2D.Kinematic;
        player.Sprite.flipY = true;
        player.Actions.Orbit.Enable();
        player.Actions.Orbit.Move.performed += Move;
        player.Actions.Orbit.Move.canceled += Move;
        player.Sprite.sprite = playerSprite;
        inputX = 0f;
    }

    public override void OnUpdate()
    {
        player.transform.RotateAround(Vector3.zero, Vector3.forward, -inputX * 300 * Time.deltaTime);
    }

    public override void OnExit()
    {
        player.Actions.Orbit.Disable();
        player.Rigid.bodyType = RigidbodyType2D.Dynamic;
        player.Sprite.flipY = false;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputX = input.x;
    }
}
