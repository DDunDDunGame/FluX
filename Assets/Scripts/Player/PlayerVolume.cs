using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerVolume
{
    private readonly Volume volume;

    public PlayerVolume(Volume volume)
    {
        this.volume = volume;
        volume.weight = 0;
    }

    public void EnableSmooth(float time)
    {
        volume.StartCoroutine(SmoothCoroutine(time, 1));
    }

    public void DisableSmooth(float time)
    {
        volume.StartCoroutine(SmoothCoroutine(time, 0));
    }

    public void EnableAndDisableSmooth(float halfTime)
    {
        void disableAction() => volume.StartCoroutine(SmoothCoroutine(halfTime, 0));
        volume.StartCoroutine(SmoothCoroutine(halfTime, 1, disableAction));
    }

    public void SetActive(bool isActive)
    {
        volume.weight = isActive ? 1 : 0;
    }

    private IEnumerator SmoothCoroutine(float time, float target)
    {
        float original = volume.weight;
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            volume.weight = Mathf.Lerp(original, target, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SmoothCoroutine(float time, float target, System.Action callback)
    {
        float original = volume.weight;
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            volume.weight = Mathf.Lerp(original, target, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        callback?.Invoke();
    }
}
