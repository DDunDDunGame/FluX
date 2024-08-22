using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialToggleGroup : MonoBehaviour
{
    private ToggleGroup toggleGroup;
    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }
}
