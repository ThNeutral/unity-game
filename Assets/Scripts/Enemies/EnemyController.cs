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

    private Dictionary<GameObject, BaseSpawner> instantiatedSpawners = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var spawner in instantiatedSpawners.Values)
        {
            spawner.AddToCounter(Time.deltaTime);
            if (spawner.IsReady())
            {
                spawner.SpawnEnemy();
            }
        }
    }

    public Dictionary<GameObject, BaseEnemy> GetInstantiatedEnemies()
    {
        IEnumerable<KeyValuePair<GameObject, BaseEnemy>> dict = new Dictionary<GameObject, BaseEnemy>();
        foreach (var spawner in instantiatedSpawners.Values) 
        {
            dict = dict.Concat(spawner.GetInstantiatedEnemies());
        }
        return dict.ToDictionary(group => group.Key, group => group.Value);
    }

    public void DealDamageTo(GameObject enemy, int damage)
    {
        foreach (var spawner in instantiatedSpawners.Values)
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

    private GameObject PlaceSpawner(GameObject spawnerPrefab, Vector3 position, Quaternion rotation)
    {
        var spawner = Instantiate(spawnerPrefab, position, rotation);
        var behavior = spawner.GetComponentInChildren<BaseSpawner>();
        instantiatedSpawners[spawner] = behavior;
        return spawner;
    }
}
