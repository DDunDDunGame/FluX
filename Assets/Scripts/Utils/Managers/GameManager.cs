using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    public float PlayTime { get { return Time.time - playStartTime; } }
    private float playStartTime;
    public int Score { get; private set; }
    public int HighScore { get; private set; }

    public bool IsPlaying { get; private set; }
    public event Action GameOverAction;

    public GameManager()
    {
        IsPlaying = false;
    }

    public void Play()
    {
        IsPlaying = true;
        playStartTime = Time.time;
        Score = 0;
    }

    public void Resume()
    {
        IsPlaying = true;
    }

    public void Pause()
    {
        IsPlaying = false;
    }

    public void GameOver()
    {
        if(Score > HighScore) HighScore = Score;
        IsPlaying = false;
        GameOverAction?.Invoke();
        GameOverAction = null;
    }

    public void AddScore(int value)
    {
        Score += value;
    }

    public void SetMaxScore(int value)
    {
        HighScore = value;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Lobby");
    }
}
