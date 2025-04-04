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
        for (int i = 0; i < navData.navPoints.Count; i++)
        {
            var navPoint = navData.navPoints[i];
            using (new Handles.DrawingScope(navPoint.color))
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPosition = Handles.PositionHandle(navPoint.position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    if (navPoint.lockedAxes.x) newPosition.x = 0;
                    if (navPoint.lockedAxes.y) newPosition.y = 0;
                    if (navPoint.lockedAxes.z) newPosition.z = 0;
                    Undo.RecordObject(navData, "Move Navigation Point");
                    navPoint.position = newPosition;
                    EditorUtility.SetDirty(navData);
                }

                Handles.DrawWireCube(newPosition, navPoint.size);
                if (i != 0)
                {
                    Handles.DrawLine(prev, newPosition);
                }
                prev = newPosition;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        var data = (NavigationData)target;

        if (GUILayout.Button("Add Nav Point"))
        {
            data.navPoints.Add(new NavigationPoint());
        }

        if (GUILayout.Button("Clear Nav Points"))
        {
            data.navPoints.Clear();
        }

        for (int i = 0; i < data.navPoints.Count; i++)
        {
            var point = data.navPoints[i];
            EditorGUILayout.LabelField("Nav Point " + i, EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Locked Axes", GUILayout.Width(100));

            EditorGUILayout.LabelField("X", GUILayout.Width(10));
            point.lockedAxes.x = EditorGUILayout.Toggle(point.lockedAxes.x, GUILayout.Width(20));

            EditorGUILayout.LabelField("Y", GUILayout.Width(10));
            point.lockedAxes.y = EditorGUILayout.Toggle(point.lockedAxes.y, GUILayout.Width(20));

            EditorGUILayout.LabelField("Z", GUILayout.Width(10));
            point.lockedAxes.z = EditorGUILayout.Toggle(point.lockedAxes.z, GUILayout.Width(20));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Draw Color", GUILayout.Width(100));
            point.color = EditorGUILayout.ColorField(point.color);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Position", GUILayout.Width(100));

            EditorGUI.BeginDisabledGroup(point.lockedAxes.x);
            EditorGUILayout.LabelField("X", GUILayout.Width(10));
            point.position.x = EditorGUILayout.FloatField(point.position.x, GUILayout.Width(50));
            if (point.lockedAxes.x) point.position.x = 0;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(point.lockedAxes.y);
            EditorGUILayout.LabelField("Y", GUILayout.Width(10));
            point.position.y = EditorGUILayout.FloatField(point.position.y, GUILayout.Width(50));
            if (point.lockedAxes.y) point.position.y = 0;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(point.lockedAxes.z);
            EditorGUILayout.LabelField("Z", GUILayout.Width(10));
            point.position.z = EditorGUILayout.FloatField(point.position.z, GUILayout.Width(50));
            if (point.lockedAxes.z) point.position.z = 0;
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Size", GUILayout.Width(100));

            EditorGUI.BeginDisabledGroup(point.lockedAxes.x);
            EditorGUILayout.LabelField("X", GUILayout.Width(10));
            point.size.x = Mathf.Max(0.001f, EditorGUILayout.FloatField(point.size.x, GUILayout.Width(50)));
            if (point.lockedAxes.x) point.size.x = 2e4f;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(point.lockedAxes.y);
            EditorGUILayout.LabelField("Y", GUILayout.Width(10));
            point.size.y = Mathf.Max(0.001f, EditorGUILayout.FloatField(point.size.y, GUILayout.Width(50)));
            if (point.lockedAxes.y) point.size.y = 2e4f;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(point.lockedAxes.z);
            EditorGUILayout.LabelField("Z", GUILayout.Width(10));
            point.size.z = Mathf.Max(0.001f, EditorGUILayout.FloatField(point.size.z, GUILayout.Width(50)));
            if (point.lockedAxes.z) point.size.z = 2e4f;
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            EditorUtility.SetDirty(target);

            if (i != data.navPoints.Count - 1)
            {
                EditorGUILayout.Space(10);
            }
        }
    }
}
