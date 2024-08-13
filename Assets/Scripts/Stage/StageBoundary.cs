using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBoundary : MonoBehaviour, IStageAttachment
{
    // Up, Down, Left, Right
    GameObject[] boundaries = new GameObject[4];

    private void Awake()
    {
        boundaries[0] = Util.FindChild(gameObject, "Top");
        boundaries[1] = Util.FindChild(gameObject, "Bottom");
        boundaries[2] = Util.FindChild(gameObject, "Left");
        boundaries[3] = Util.FindChild(gameObject, "Right");
    }

    public void ChangeStage(Define.Stage stage)
    {
        if(stage == Define.Stage.Jump)
        {
            boundaries[0].SetActive(false);
            boundaries[1].SetActive(false);
        }
        else
        {
            boundaries[0].SetActive(true);
            boundaries[1].SetActive(true);
            boundaries[2].SetActive(true);
            boundaries[3].SetActive(true);
        }
    }
}