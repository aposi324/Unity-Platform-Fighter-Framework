using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AnimationWindow : EditorWindow
{
    private GameObject selectedObject;
   // private AnimationClip clip = new AnimationClip();
    [MenuItem("Window/Animation Window")]
    
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimationWindow));
    }

  
  
    private void OnGUI()
    {
        // Window code goes here
        //selectedObject = Selection.activeTransform.gameObject;
        //Debug.Log(selectedObject.name);
        GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
        int selected = 0;
        string[] options = new string[] { "Stand", "Fall", "Fair" };
        EditorGUILayout.Popup("Animation", selected, options);
                GUILayout.Box("1");
            GUILayout.EndVertical();
            
            GUILayout.BeginVertical();
                GUILayout.Box("2");
                GUILayout.Box("3");
        GUILayout.EndHorizontal();
    }
}
