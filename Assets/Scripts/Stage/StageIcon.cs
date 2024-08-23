using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIcon : MonoBehaviour, IStageAttachment
{
    // 0. Shooting, 1. Mouse, 2. Run, 3. Jump, 4. Orbit, 5. Barrier
    [SerializeField] private Sprite[] stageIcons;
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void ChangeStage(Define.Stage stage)
    {
        if(stage == Define.Stage.Boss || stage == Define.Stage.None)
        {
            sprite.sprite = null;
            return;
        }
        sprite.sprite = stageIcons[(int)stage];
    }
}
