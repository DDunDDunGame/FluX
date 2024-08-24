using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStageAttachment
{
    void EnterStage(Define.Stage stage);
    void ExitStage(Define.Stage stage);
}
