using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerVolume
{
    private readonly Player player;
    private readonly Volume volume;

    public PlayerVolume(Player player, Volume volume)
    {
        this.player = player;
        this.volume = volume;
        volume.weight = 0;
    }

    public void EnableSmooth(float time)
    {
        player.StartCoroutine(SmoothCoroutine(time, 1));
    }

    public void DisableSmooth(float time)
    {
        player.StartCoroutine(SmoothCoroutine(time, 0));
    }

    public void SetActive(bool isActive)
    {
        volume.weight = isActive ? 1 : 0;
    }

    public void SetActive(bool isActive, float time)
    {
        player.StartCoroutine(SetActiveCoroutine(isActive, time));
    }

    private IEnumerator SetActiveCoroutine(bool isActive, float time)
    {
        yield return new WaitForSeconds(time);
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
}
