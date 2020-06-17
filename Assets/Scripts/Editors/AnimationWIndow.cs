using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AnimationWindow : EditorWindow
{
    [MenuItem("Window/Animation Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimationWindow));
    }
    private void OnGUI()
    {
        // Window code goes here
    }
}
