using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreenKeyboardOculus : MonoBehaviour
{
    private TouchScreenKeyboard overlayKeyboard;
    public static string inputText = "";

    
    public void OpenKey()
    {
        overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);

    }
}
