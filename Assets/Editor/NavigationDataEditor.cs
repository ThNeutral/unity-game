using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavigationData))]
public class NavigationDataEditor : Editor
{
    private Color drawColor = Color.yellow;

    private void OnEnable()
    {
        SceneView.duringSceneGui -= SceneGUI;
        SceneView.duringSceneGui += SceneGUI;
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= SceneGUI;
    }

    private void SceneGUI(SceneView sv)
    {
        var navData = (NavigationData)target;
        if (navData.navPoints.Count == 0) return;

        Vector3 prev = Vector3.zero;
        using (new Handles.DrawingScope(drawColor))
        {
            for (int i = 0; i < navData.navPoints.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPoint = Handles.PositionHandle(navData.navPoints[i], Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(navData, "Move Navigation Point");
                    navData.navPoints[i] = newPoint;
                    EditorUtility.SetDirty(navData);
                }

                Handles.DrawWireCube(navData.navPoints[i], Vector3.one * 0.5f);
                if (i != 0)
                {
                    Handles.DrawLine(prev, navData.navPoints[i]);
                }
                prev = navData.navPoints[i];
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var navData = (NavigationData)target;

        var newDrawColor = EditorGUILayout.ColorField("Draw Color", drawColor);
        if (newDrawColor != drawColor) SceneGUI(null);
        drawColor = newDrawColor;


        if (GUILayout.Button("Add Navigation Point"))
        {
            Undo.RecordObject(navData, "Add Navigation Point");
            navData.navPoints.Add(Vector3.zero);
            EditorUtility.SetDirty(navData);
        }

        if (GUILayout.Button("Clear Navigation Points"))
        {
            Undo.RecordObject(navData, "Clear Navigation Points");
            navData.navPoints.Clear();
            EditorUtility.SetDirty(navData);
        }
    }
}
