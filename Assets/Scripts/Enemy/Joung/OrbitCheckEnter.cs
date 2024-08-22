using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCheckEnter : MonoBehaviour
{
    public bool checkEnterPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            checkEnterPlayer = true;
        }
    }
}
