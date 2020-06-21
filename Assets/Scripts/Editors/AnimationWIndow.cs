using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
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
        selectedObject = Selection.activeTransform.gameObject;
        //Debug.Log(selectedObject.name);
        GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
        int selected = 0;
        string[] options = new string[] { "Stand", "Fall", "Fair" };
        EditorGUILayout.Popup("Animation", selected, options);
                GUILayout.Box("1");
            GUILayout.EndVertical();
            
            GUILayout.BeginVertical();
                DrawOnGUISprite(selectedObject.GetComponent<SpriteRenderer>().sprite);
                Canvas canvas = new Canvas();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GUILayout.EndHorizontal();
    }

    void DrawOnGUISprite(Sprite aSprite)
    {
        Rect c = aSprite.rect;
        float spriteW = c.width;
        float spriteH = c.height;
        Rect rect = GUILayoutUtility.GetRect(spriteW, spriteH);
        rect = new Rect(0f,0f,spriteW,spriteH);
        if (Event.current.type == EventType.Repaint)
        {
            var tex = aSprite.texture;
            c.xMin /= tex.width;
            c.xMax /= tex.width;
            c.yMin /= tex.height;
            c.yMax /= tex.height;
            GUI.DrawTextureWithTexCoords(rect, tex, c);
        }
    }

}
