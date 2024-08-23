using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GameOverUI : MonoBehaviour
{
    private TextMeshProUGUI playTimeText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI highScoreText;
    private VolumeHelper volume;
    [SerializeField] private float volumeChangeTime = 1;

    private void Awake()
    {
        InitVariables();
        Managers.Game.GameOverAction -= Show;
        Managers.Game.GameOverAction += Show;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Managers.Game.Restart();
        }
    }

    private void InitVariables()
    {
        playTimeText = Util.FindChild<TextMeshProUGUI>(gameObject, "PlayTimeText", true);
        scoreText = Util.FindChild<TextMeshProUGUI>(gameObject, "ScoreText", true);
        highScoreText = Util.FindChild<TextMeshProUGUI>(gameObject, "HighScoreText", true);
        volume = new VolumeHelper(this, Util.FindChild<Volume>(gameObject, "GameOverVolume"));
    }

    private void Show()
    {
        gameObject.SetActive(true);
        volume.EnableSmooth(volumeChangeTime);

        int minute = (int)Managers.Game.PlayTime / 60;
        string minuteString = minute < 10 ? $"0{minute}" : $"{minute}";
        int second = (int)Managers.Game.PlayTime % 60;
        string secondString = second < 10 ? $"0{second}" : $"{second}";
        playTimeText.text = $"Play Time: {minuteString}:{secondString}";

        int score = Managers.Game.Score;
        string scoreString = "";
        while (score > 0)
        {
            string part = (score % 1000).ToString("D3");
            if (score < 1000)
                scoreString = part.TrimStart('0') + scoreString;
            else
                scoreString = "," + part + scoreString;
            score /= 1000;
        }
        if (scoreString == "") scoreString = "0";
        scoreText.text = scoreString;
        bool isHighScore = score == Managers.Game.HighScore;
        highScoreText.gameObject.SetActive(isHighScore);
    }
}
