using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(NavigationData))]
public class NavigationDataEditor : Editor
{
    private EnemyType selectedEnemyType = EnemyType.BASE;
    private void OnEnable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sv)
    {
        var navData = (NavigationData)target;
        var path = navData.paths.FirstOrDefault(p => p.type == selectedEnemyType);
        if (path == null) return;

        Vector3 prev = Vector3.zero;
        for (int i = 0; i < path.path.Count; i++)
        {
            var navPoint = path.path[i];
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
        selectedEnemyType = (EnemyType)EditorGUILayout.EnumPopup("Select Enemy Type", selectedEnemyType);

        var path = data.paths.FirstOrDefault(p => p.type == selectedEnemyType);
        if (path == null)
        {
            path = new NavigationPath() { path = new(), type = selectedEnemyType };
            data.paths.Add(path);
        }

        var options = path.path.Select((_, index) => index.ToString()).ToArray();
        for (int i = 0; i < path.path.Count; i++)
        {
            var point = path.path[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Nav Point ", EditorStyles.boldLabel, GUILayout.Width(70));
            var newI = EditorGUILayout.Popup(i, options, GUILayout.Width(40));
            if (newI != i)
            {
                (data.paths[newI], data.paths[i]) = (data.paths[i], data.paths[newI]);  
            }
            if (GUILayout.Button("Delete"))
            {
                data.paths.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

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

            if (i != data.paths.Count - 1)
            {
                EditorGUILayout.Space(10);
            }
        }

        if (GUILayout.Button("Add Nav Point"))
        {
            path.path.Add(new NavigationPoint());
        }

        if (GUILayout.Button("Clear Nav Points"))
        {
            path.path.Clear();
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}
