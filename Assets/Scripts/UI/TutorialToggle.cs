using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialToggle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject tutorial;
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(Toggle);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void Toggle(bool value)
    {
        Debug.Log(gameObject.name + " " + value);
        tutorial.SetActive(value);
    }
}