using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseTutorial : MonoBehaviour
{
    [SerializeField] private Vector2 maxPos;
    [SerializeField] private Vector2 minPos;
    PlayerActions actions;

    private void Awake()
    {
        actions = new PlayerActions();
    }

    private void OnEnable()
    {
        actions.Mouse.Enable();
    }

    private void OnDisable()
    {
        actions.Mouse.Disable();
    }

    private void Update()
    {
        Vector2 mousePos = actions.Mouse.Move.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.x = Mathf.Clamp(worldPos.x, minPos.x, maxPos.x);
        worldPos.y = Mathf.Clamp(worldPos.y, minPos.y, maxPos.y);
        transform.position = worldPos;
    }
}

