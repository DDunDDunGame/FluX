using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEffect : MonoBehaviour
{
    private ParticleSystem barrierEffect;

    private void Awake()
    {
        barrierEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        barrierEffect.Play();
    }
}
