using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnerPrefab;

    private Dictionary<BaseSpawner, bool> instantiatedSpawners = new();

    private GenerationDataProvider generationDataProvider;
    private TowerController towerController;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        towerController = FindFirstObjectByType<TowerController>();
        generationDataProvider = FindFirstObjectByType<GenerationDataProvider>();
        playerController = FindFirstObjectByType<PlayerController>();
        GenerateSpawners();
    }

    public void GenerateSpawners()
    {
        foreach (var spawnerPoint in generationDataProvider.SpawnerGenerationData.generationPoints)
        {
            PlaceSpawner(spawnerPoint.prefab, spawnerPoint.position, Quaternion.identity);
        }
    }

    public List<MonoBehaviour> DealDamageInArea(Vector3 center, float radius, int damage)
    {
        var towers = towerController.GetTowersInSphere(center, radius);
        var destroyed = new List<MonoBehaviour>();
        foreach (var tower in towers)
        {
            if (tower.RecieveDamage(damage))
            {
                destroyed.Add(tower);
            }
        }
        if (Vector3.Distance(playerController.transform.position, center) < radius)
        {
            if (playerController.RecieveDamage(damage))
            {
                destroyed.Add(playerController);
            }
        }
        return destroyed;
    }

    public Dictionary<BaseEnemy, bool> GetInstantiatedEnemies()
    {
        IEnumerable<KeyValuePair<BaseEnemy, bool>> dict = new Dictionary<BaseEnemy, bool>();
        foreach (var spawner in instantiatedSpawners.Keys) 
        {
            dict = dict.Concat(spawner.GetInstantiatedEnemies());
        }
        return dict.ToDictionary(group => group.Key, group => group.Value);
    }

    public BaseEnemy GetClosestEnemy(Vector3 point) {
        float minDistance = float.MaxValue;
        BaseEnemy closestEnemy = null;
        
        foreach (var enemy in GetInstantiatedEnemies().Keys)
        {
            var distance = Vector3.Distance(point, enemy.transform.position);
            if (distance < minDistance)
            {
                closestEnemy = enemy;
                minDistance = distance;
            }
        }

        return closestEnemy;
    }

    public List<NavigationPoint> GetRoute(EnemyType type, Vector3 startPos)
    {
        return generationDataProvider.NavigationProvider.GetRemainingRoute(type, startPos);
    }

    public void DealDamageTo(BaseEnemy enemy, int damage)
    {
        foreach (var spawner in instantiatedSpawners.Keys)
        {
            if (spawner.Contains(enemy))
            {
                spawner.DealDamageToEnemy(enemy, damage);
                return;
            }
        }
    }

    public GameObject PlaceSpawner(Vector3 position, Quaternion rotation)
    {
        return PlaceSpawner(spawnerPrefab, position, rotation);
    }

    public GameObject PlaceSpawner(GameObject spawnerPrefab, Vector3 position, Quaternion rotation)
    {
        var spawner = Instantiate(spawnerPrefab, position, rotation);
        instantiatedSpawners[spawner.GetComponentInChildren<BaseSpawner>()] = true;
        return spawner;
    }
}
