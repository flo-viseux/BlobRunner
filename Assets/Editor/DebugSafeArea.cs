using UnityEditor;
using UnityEngine;

public class DebugSafeArea : EditorWindow
{
    [MenuItem("Window/Debug Safe Area Window")]
    private static void ShowWindow()
    {
        var window = GetWindow<DebugSafeArea>();
        window.titleContent = new GUIContent("TITLE");
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Screen width : " + Screen.currentResolution.width);
        EditorGUILayout.LabelField("Screen height : " + Screen.currentResolution.height);
        EditorGUILayout.LabelField("Safe Area y : " + Screen.safeArea.y);
        EditorGUILayout.LabelField("Safe Area yMin : " + Screen.safeArea.yMin);
        EditorGUILayout.LabelField("Safe Area yMax : " + Screen.safeArea.yMax);
        EditorGUILayout.LabelField("Safe Area x : " + Screen.safeArea.x);
        EditorGUILayout.LabelField("Safe Area xMin : " + Screen.safeArea.xMin);
        EditorGUILayout.LabelField("Safe Area xMax : " + Screen.safeArea.xMax);
    }
}
