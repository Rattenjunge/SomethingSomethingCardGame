using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitOnClick : MonoBehaviour
{
    public void QuitGame() {
        Application.Quit();
       // UnityEditor.EditorApplication.isPlaying = false;
    }
}
