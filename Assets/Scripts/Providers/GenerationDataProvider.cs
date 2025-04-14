using UnityEngine;
using System.Collections;

public class GenerationDataProvider : MonoBehaviour
{
    public NavigationProvider NavigationProvider { get; private set; }
    public SpawnerGenerationData SpawnerGenerationData { get; private set; }

    [SerializeField]
    private MapGenerationData mapGenerationData;

    private void Awake()
    {
        NavigationProvider = new();
        NavigationProvider.navData = mapGenerationData.navigationData;
        SpawnerGenerationData = mapGenerationData.spawnerGenerationData;
    }
}
