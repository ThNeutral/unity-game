using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField]
    private Bounds spawnZone;
    [SerializeField] 
    private Bounds spawnExclusionZone;

    [SerializeField]
    private float spawnDelay = 0.2f;
    private float counter = 0f;

    [SerializeField]
    private GameObject enemy;

    private EnemyController enemyController;
    private Dictionary<BaseEnemy, bool> instantiatedEnemies = new();

    [SerializeField]
    private int maxNumberOfSpawns = 5;
    private int currentNumberOfSpawns;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = FindFirstObjectByType<EnemyController>();
    }
    // Update is called once per frame
    void Update()
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
        if (maxNumberOfSpawns != -1 && currentNumberOfSpawns >= maxNumberOfSpawns) return;
        currentNumberOfSpawns += 1;
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
            if (enemy.ReceiveDamage(damage)) 
            { 
                instantiatedEnemies.Remove(enemy);
            }
        }
    }
}
