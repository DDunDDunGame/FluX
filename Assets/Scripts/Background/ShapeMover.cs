using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float interval;
    private float timer;
    private float sign;

    private void Awake()
    {
        timer = Time.time;
        sign = 1f;
    }

    private void Update()
    {
        if (Time.time >= interval + timer)
        {
            timer = Time.time;
            sign = -sign;
        }
        transform.position = Vector3.Lerp(transform.position, transform.position + sign * transform.up, speed);
    }
}
