using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MapGenerationData", menuName = "Scriptable Objects/Map Generation Data")]
public class MapGenerationData : ScriptableObject
{
    public NavigationData navigationData;
    public SpawnerGenerationData spawnerGenerationData;
}
