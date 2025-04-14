using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerGenerationData", menuName = "Scriptable Objects/Spawner Generation Data")]
public class SpawnerGenerationData : ScriptableObject
{
    public List<SpawnerGenerationPoint> generationPoints = new();
}
