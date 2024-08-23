using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RoundImage : MonoBehaviour
{
    [SerializeField] private Sprite[] roundSprites;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetRoundImage(int round)
    {
        round--;
        if (round < 0 || round >= roundSprites.Length)
        {
            image.sprite = null;
        }

        image.sprite = roundSprites[round];
    }
}
