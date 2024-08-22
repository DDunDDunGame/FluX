using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeHelper
{
    private readonly MonoBehaviour owner;
    private readonly Volume volume;

    public VolumeHelper(MonoBehaviour owner, Volume volume)
    {
        this.owner = owner;
        this.volume = volume;
        volume.weight = 0;
    }

    public void EnableSmooth(float time, Action callback = null)
    {
        owner.StartCoroutine(SmoothCoroutine(time, 1, callback));
    }

    public void DisableSmooth(float time, Action callback = null)
    {
        owner.StartCoroutine(SmoothCoroutine(time, 0, callback));
    }

    public void SetActive(bool isActive)
    {
        volume.weight = isActive ? 1 : 0;
    }
    public void SetActive(bool isActive, float time, Action callback)
    {
        owner.StartCoroutine(SetActiveCoroutine(isActive, time, callback));
    }

    private IEnumerator SetActiveCoroutine(bool isActive, float time, Action callback)
    {
        volume.weight = isActive ? 1 : 0;
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    private IEnumerator SmoothCoroutine(float time, float target, Action callback)
    {
        float original = volume.weight;
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            volume.weight = Mathf.Lerp(original, target, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        volume.weight = target;
        callback?.Invoke();
    }
}
