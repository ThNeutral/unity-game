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
    // Start is called before the first frame update
    void Start()
    {
        towerController = FindObjectOfType<TowerController>();
        PlaceSpawner(transform.position + new Vector3(10, 0, 8), transform.rotation);
        //PlaceSpawner(transform.position + new Vector3(8, 0, 10), transform.rotation);
        //PlaceSpawner(transform.position + new Vector3(6, 0, 10), transform.rotation);
        //PlaceSpawner(transform.position + new Vector3(10, 0, 6), transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
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

    public BaseTower GetTarget(BaseEnemy enemy)
    {
        var values = towerController.GetInstantiatedTowers().Keys.ToList();
        return values[UnityEngine.Random.Range(0, values.Count)];
    }

    public bool IsValidTarget(BaseEnemy enemy, BaseTower target)
    {
        var values = towerController.GetInstantiatedTowers().Values.ToList();
        return values.Contains(target);
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
        behaviour.SetEnemyController(this);
        instantiatedSpawners[behaviour] = true;
        return spawner;
    }
}
