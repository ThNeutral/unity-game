using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SpawnerGenerationData))]
public class SpawnerGenerationDataEditor : Editor
{
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
        var generationData = (SpawnerGenerationData)target;
        if (generationData.generationPoints.Count == 0) return;

        for (int i = 0; i < generationData.generationPoints.Count; i++)
        {
            var generationPoint = generationData.generationPoints[i];
            using (new Handles.DrawingScope(generationPoint.color))
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPosition = Handles.PositionHandle(generationPoint.position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(generationData, "Move Navigation Point");
                    generationPoint.position = newPosition;
                    EditorUtility.SetDirty(generationData);
                }

                Handles.DrawWireCube(newPosition, Vector3.one * 0.5f);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        var generationData = (SpawnerGenerationData)target;

        for (int i = 0; i < generationData.generationPoints.Count; i++)
        {
            var generationPoint = generationData.generationPoints[i];

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Generation Point {i}", EditorStyles.boldLabel, GUILayout.Width(140));
            if (GUILayout.Button("Delete"))
            {
                generationData.generationPoints.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();

            generationPoint.color = EditorGUILayout.ColorField("Draw Color", generationPoint.color);
            generationPoint.position = EditorGUILayout.Vector3Field("Spawner Position", generationPoint.position);
            var newPrefab = (GameObject)EditorGUILayout.ObjectField(
                "Spawner Prefab",
                generationPoint.prefab,
                typeof(GameObject),
                true
                );
            if (newPrefab != null && newPrefab.GetComponentInChildren<BaseSpawner>() != null)
            {
                generationPoint.prefab = newPrefab;
            }
            else
            {
                EditorGUILayout.HelpBox("Prefab must be set and contain BaseSpawner in its children", MessageType.Warning);
            }

        }

        if (GUILayout.Button("Add Generation Points"))
        {
            generationData.generationPoints.Add(new SpawnerGenerationPoint());
        }

        if (GUILayout.Button("Clear Generation Points"))
        {
            generationData.generationPoints.Clear();
        }
    }
}
