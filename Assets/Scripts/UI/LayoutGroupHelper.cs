using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutGroupHelper : MonoBehaviour
{
    [SerializeField] private Image childPrefab;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;
    private readonly List<Image> children = new();
    private int index = -1;

    public void Init(int childCount)
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < childCount; i++)
        {
            children.Add(Instantiate(childPrefab, transform));
            SetActive(i, false);
        }
    }

    public void Decrement(int count)
    {
        for(int i = 0; i < count; i++)
        {
            if (index < 0) { return; }
            SetActive(index--, false);
        }
    }

    public void Increment(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (index >= children.Count) { return; }
            SetActive(++index, true);
        }
    }

    private void SetActive(int index, bool active)
    {
        children[index].color = active ? activeColor : inactiveColor;
    }
}
