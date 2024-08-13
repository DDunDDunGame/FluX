using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public static GameObject MapCreate(GameObject obj, Vector3 pos)
    {
        GameObject parent = GameObject.Find("Map");
        return Instantiate(obj, pos, Quaternion.identity, parent.transform);
    }

    public static bool MapDestroy()
    {
        GameObject parent = GameObject.Find("Map");
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
        return false;
    }
}
