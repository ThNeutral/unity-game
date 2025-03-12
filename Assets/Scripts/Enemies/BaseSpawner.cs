using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField]
    protected Bounds spawnZone;
    [SerializeField]
    protected Bounds spawnExclusionZone;
    [SerializeField]
    protected float spawnDelay = 0.2f;
    [SerializeField]
    protected GameObject enemy;

    [SerializeField]
    protected int maxNumberOfSummons = 2;

    protected EnemyController enemyController;
    protected Dictionary<BaseEnemy, bool> instantiatedEnemies = new();
    protected float spawnCounter = 0f;
    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }
    // Update is called once per frame
    private void Update()
    {
        if (instantiatedEnemies.Count >= maxNumberOfSummons) return;

        spawnCounter += Time.deltaTime;
        while (spawnCounter > spawnDelay) 
        {
            SpawnEnemy();
            spawnCounter -= spawnDelay;
        }
    }
    protected void Initialize()
    {
        enemyController = FindFirstObjectByType<EnemyController>();
    }
    public void SpawnEnemy(bool isControlled = false)
    {
        const int maxCount = 100;
        int count = 0;
        Vector3 spawnPoint;
        do
        {
            count += 1;
            if (count > maxCount)
            {
                Debug.LogError("Failed to generate a spawn point in 100 tries. Check spawnZone and spawnExclusionZone Bounds");
                spawnPoint = Vector3.zero;
                break;
            }

            var x = UnityEngine.Random.Range(spawnZone.min.x, spawnZone.max.x);
            var y = UnityEngine.Random.Range(spawnZone.min.y, spawnZone.max.y);
            var z = UnityEngine.Random.Range(spawnZone.min.z, spawnZone.max.z);

            spawnPoint = new Vector3(x, y, z);
        } while (spawnExclusionZone.Contains(spawnPoint));

        var clonePosition = transform.position + spawnPoint;
        var cloneRotation = Quaternion.identity;

        var instantiatedEnemy = Instantiate(enemy, clonePosition, cloneRotation);
        var behavoiur = instantiatedEnemy.GetComponent<BaseEnemy>();

        behavoiur.SetIsControlled(isControlled);

        instantiatedEnemies[behavoiur] = true;

        spawnCounter = 0;
    }
    public Dictionary<BaseEnemy, bool> GetInstantiatedEnemies()
    {
        return instantiatedEnemies;
    }
    public bool Contains(BaseEnemy enemy)
    {
        return instantiatedEnemies.ContainsKey(enemy);
    }
    public void DealDamageToEnemy(BaseEnemy enemy, int damage)
    {
        if (instantiatedEnemies.ContainsKey(enemy))
        {
            var isDestroyed = enemy.DealDamage(damage);
            if (isDestroyed) 
            { 
                instantiatedEnemies.Remove(enemy);
            }
        }
    }
    public void AddEnemies(Dictionary<BaseEnemy, bool> enemies)
    {
        instantiatedEnemies = instantiatedEnemies.Concat(enemies).ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}
