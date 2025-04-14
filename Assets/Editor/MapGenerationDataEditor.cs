using UnityEngine;
using UnityEditor;

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
            "Navigation Data"
            );

        mapGenerationData.spawnerGenerationData = NestedEditor(
            mapGenerationData.spawnerGenerationData,
            ref spawnerGenerationDataEditor,
            "Spawner Generation Data"
            );
    }

    private SO NestedEditor<SO, E>(SO oldSO, ref E editor, string label)
        where SO : ScriptableObject
        where E : Editor 
    {
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
            editor = (E)CreateEditor(newSO);
        }
        else if (newSO != oldSO)
        {
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
