using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.anyKey.IsPressed())
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
