using System;
using System.Collections;
using System.Collections.Generic;
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

    protected EnemyController enemyController;
    protected TowerController towerController;
    protected Dictionary<BaseEnemy, bool> instantiatedEnemies = new();
    protected float counter = 0f;
    // Start is called before the first frame update
    private void Start()
    {
    }
    // Update is called once per frame
    private void Update()
    {
        counter += Time.deltaTime;
        while (counter > spawnDelay) 
        {
            SpawnEnemy();
            counter -= spawnDelay;
        }
    }
    public void SpawnEnemy()
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
        var cloneRotation = Quaternion.Euler(0, 0, 0);

        var instantiatedEnemy = Instantiate(enemy, clonePosition, cloneRotation);
        var behavoiur = instantiatedEnemy.GetComponentInChildren<BaseEnemy>();

        behavoiur.SetEnemyController(enemyController);
        behavoiur.SetTowerController(towerController);

        instantiatedEnemies[behavoiur] = true;

        counter = 0;
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
            var isDestroyed = enemy.RecieveDamage(damage);
            if (isDestroyed) 
            { 
                instantiatedEnemies.Remove(enemy);
            }
        }
    }
    public void SetEnemyController(EnemyController controller)
    {
        enemyController = controller;
    }
    public void SetTowerController(TowerController controller)
    {
        towerController = controller;
    }
}
