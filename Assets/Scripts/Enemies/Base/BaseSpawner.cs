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
    protected float counter = 0f;

    [SerializeField]
    protected GameObject enemy;

    protected EnemyController enemyController;
    protected Dictionary<BaseEnemy, bool> instantiatedEnemies = new();

    [SerializeField]
    protected int maxNumberOfSpawns = 5;
    protected int CurrentNumberOfSpawns => instantiatedEnemies.Count;

    protected bool isControlled = false;

    // Start is called before the first frame update
    protected void Start()
    {
        enemyController = FindFirstObjectByType<EnemyController>();
    }
    // Update is called once per frame
    protected void Update()
    {
        if (isControlled) return;
        counter += Time.deltaTime;
        while (counter > spawnDelay) 
        {
            SpawnEnemy();
            counter -= spawnDelay;
        }
    }
    public void SpawnEnemy()
    {
        if (maxNumberOfSpawns != -1 && CurrentNumberOfSpawns >= maxNumberOfSpawns) return;
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
