using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Util : MonoBehaviour
{
    public static GameObject FindChild(GameObject parent, string name = null, bool recursive = false)
    {
        Transform temp = FindChild<Transform>(parent, name, recursive);
        if (temp != null)
            return temp.gameObject;
        return null;
    }
    public static T FindChild<T>(GameObject parent, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (parent == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || child.name == name)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in parent.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static GameObject CreateObjToParent(GameObject childObj, Vector3 pos, GameObject parentObj)
    {
        return Instantiate(childObj, pos, Quaternion.identity, parentObj.transform);
    }

    public static bool DestoryObjFromParent(GameObject parentObj)
    {
        foreach (Transform child in parentObj.transform)
        {
            Destroy(child.gameObject);
        }
        return false;
    }

    public static List<T> Shuffle<T>(List<T> values)
    {
        var rand = new System.Random();
        var shuffled = values.OrderBy(_ => rand.Next()).ToList();

        return shuffled;
    }
}
