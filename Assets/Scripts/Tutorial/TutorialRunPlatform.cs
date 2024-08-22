using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RunPlatform))]
public class TutorialRunPlatform : MonoBehaviour
{
    private RunPlatform runPlatform;
    private Vector2 initPos;
    private float minX = -5.3f;
    private float maxX = 5.3f;

    private void Awake()
    {
        runPlatform = GetComponent<RunPlatform>();
        initPos = transform.position;
    }

    private void Update()
    {
        if (transform.position.x < minX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }
    }

    private void OnEnable()
    {
        runPlatform.Move(initPos, Vector2.left);
    }
}
