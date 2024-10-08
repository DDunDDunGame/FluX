using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    AsyncOperation op;
    TextMeshProUGUI highScoreText;
    [SerializeField] private Transform tutorialStage;
    [SerializeField] private Transform border;
    [SerializeField] private TutorialToggle firstTutorial;

    private void Awake()
    {
        highScoreText = Util.FindChild<TextMeshProUGUI>(gameObject, "HighScoreText", true);
        SetHighScoreText(Managers.Game.HighScore);
    }

    private void Start()
    {
        firstTutorial.Toggle(true);
        firstTutorial.GetComponent<Toggle>().isOn = true;
    }

    private void SetHighScoreText(int highScore)
    {
        string scoreString = "";
        while (highScore > 0)
        {
            string part = (highScore % 1000).ToString("D3");
            if (highScore < 1000)
                scoreString = part.TrimStart('0') + scoreString;
            else
                scoreString = "," + part + scoreString;
            highScore /= 1000;
        }
        if (scoreString == "") scoreString = "0";
        highScoreText.text = scoreString;
    }


    private void Update()
    {
        if(Keyboard.current.bKey.wasPressedThisFrame && op == null)
        {
            StartCoroutine(LoadGameScene());
        }
    }

    private IEnumerator LoadGameScene()
    {
        op = SceneManager.LoadSceneAsync("Game");
        SoundManager.Instance.StopLoopSound("FLUX MAIN");
        op.allowSceneActivation = false;
        bool isFlying = false;

        while (!op.isDone)
        {
            if (op.progress >= 0.9f && !isFlying)
            {
                isFlying = true;
                FlyDownAndUp();
            }

            yield return null;
        }
    }

    private void FlyDownAndUp()
    {
        foreach(Transform child in transform)
        {
            child.DOMoveY(child.position.y - 1, 0.4f).SetEase(Ease.OutQuad);
        }
        tutorialStage.DOMoveY(tutorialStage.position.y - 0.7f, 0.4f).SetEase(Ease.OutQuad);
        border.DOMoveY(tutorialStage.position.y - 0.7f, 0.4f).SetEase(Ease.OutQuad).OnComplete(FlyUp);
    }

    private void FlyUp()
    {
        foreach (Transform child in transform)
        {
            child.DOMoveY(child.transform.position.y + 15, 0.5f).SetEase(Ease.InQuad)
                .OnComplete(() => op.allowSceneActivation = true);
        }
        tutorialStage.DOMoveY(tutorialStage.transform.position.y + 15, 0.5f).SetEase(Ease.InQuad);
        border.DOMoveY(tutorialStage.transform.position.y + 15, 0.5f).SetEase(Ease.InQuad);
    }
}
