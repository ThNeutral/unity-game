using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnerPrefab;

    private Dictionary<BaseSpawner, bool> instantiatedSpawners = new();

    private TowerController towerController;
    private PlayerController playerController;

    private Dictionary<BaseEnemy, BaseTower> targets = new();

    private BaseSpawner defaultSpawner;

    private IEnemyAimingStrategy aimingStrategy;
    // Start is called before the first frame update
    void Start()
    {
        aimingStrategy = new ClosestTowerEnemyAimingStrategy();
        playerController = FindFirstObjectByType<PlayerController>();
        towerController = FindObjectOfType<TowerController>();

        defaultSpawner = PlaceDisabledSpawner(spawnerPrefab, Vector3.zero, Quaternion.identity).GetComponentInChildren<BaseSpawner>();
        PlaceSpawner(Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        targets = aimingStrategy.GetTargets(GetInstantiatedEnemies(), towerController.GetInstantiatedTowers());
    }

    public void MoveEnemiesToDefaultSpawner(BaseSpawner spawner)
    {
        if (!instantiatedSpawners.TryGetValue(spawner, out var _))
        {
            Debug.LogError("Tried to move enemies from non existent spawner");
            return;
        }

        defaultSpawner.AddEnemies(spawner.GetInstantiatedEnemies());
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

    public MonoBehaviour GetTarget(BaseEnemy enemy)
    {
        var isValid = targets.TryGetValue(enemy, out var tower);
        if (!isValid) return null;

        var distanceToEnemy = Vector3.Distance(enemy.transform.position, tower.transform.position);
        var distanceToPlayer = Vector3.Distance(enemy.transform.position, playerController.transform.position);

        if (distanceToPlayer < distanceToEnemy) return playerController;
        return tower;
    }

    public bool IsValidTarget(BaseEnemy enemy, MonoBehaviour target)
    {
        if (target == null) return false;
        switch (target)
        {
            case BaseTower:
                {
                    var res = targets.TryGetValue(enemy, out var tower);
                    return res && tower == target;
                }
            case PlayerController:
                {
                    return true;
                }
            default:
                {
                    return false;
                }
        }
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
        var behaviour = spawner.GetComponentInChildren<BaseSpawner>();
        instantiatedSpawners[behaviour] = true;
        return spawner;
    }

    private GameObject PlaceDisabledSpawner(GameObject spawnerPrefab, Vector3 position, Quaternion rotation)
    {
        var spawner = PlaceSpawner(spawnerPrefab, position, rotation);
        spawner.SetActive(false);
        return spawner;
    }
    public void RegisterSpawner(BaseSpawner spawner)
    {
        if (instantiatedSpawners.TryGetValue(spawner, out bool _))
        {
            Debug.LogError("Tried to register spawner that is already tracked");
            return;
        }

        instantiatedSpawners[spawner] = true;
    }
    public void UnregisterSpawner(BaseSpawner spawner)
    {
        if (!instantiatedSpawners.TryGetValue(spawner, out bool _))
        {
            Debug.LogError("Tried to unregister spawner that is already tracked");
            return;
        }

        instantiatedSpawners.Remove(spawner);
    }
}
