using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering.PostProcessing;

[CustomEditor(typeof(MapGenerationData))]
public class MapGenerationDataEditor : Editor
{
    private NavigationDataEditor navigationDataEditor;
    private SpawnerGenerationDataEditor spawnerGenerationDataEditor;

    public override void OnInspectorGUI()
    {
        var mapGenerationData = (MapGenerationData)target;

        mapGenerationData.navigationData = NestedEditor(
            mapGenerationData.navigationData,
            ref navigationDataEditor,
            "Navigation Data",
            out bool hasNavigationDataChanged
            );

        mapGenerationData.spawnerGenerationData = NestedEditor(
            mapGenerationData.spawnerGenerationData,
            ref spawnerGenerationDataEditor,
            "Spawner Generation Data",
            out bool hasSpawnGenerationDataChanged
            );

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);

    }

    private SO NestedEditor<SO, E>(SO oldSO, ref E editor, string label, out bool hasChanged)
        where SO : ScriptableObject
        where E : Editor 
    {
        hasChanged = false;
        var newSO = (SO)EditorGUILayout.ObjectField(label, oldSO, typeof(SO), true);
        if (newSO == null)
        {
            if (editor != null)
            {
                DestroyImmediate(editor);
                editor = null;
            }
            EditorGUILayout.HelpBox($"Set {label} instance to edit it!", MessageType.Warning);
        }
        else if (editor == null && newSO != null) 
        {
            hasChanged = true;
            editor = (E)CreateEditor(newSO);
        }
        else if (newSO != oldSO)
        {
            hasChanged = true;
            if (editor != null)
            {
                DestroyImmediate(editor);
            }
            editor = (E)CreateEditor(newSO);
        }

        editor?.OnInspectorGUI();
        return newSO;
    }

    private void OnDestroy()
    {
        if (navigationDataEditor != null)
        {
            DestroyImmediate(navigationDataEditor);
        }

        if (spawnerGenerationDataEditor != null)
        {
            DestroyImmediate(spawnerGenerationDataEditor);
        }
    }
}
