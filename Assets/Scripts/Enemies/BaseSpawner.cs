using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField]
    private Vector3 spawnSize;
    [SerializeField]
    private float spawnDelay = 0.2f;
    [SerializeField]
    private GameObject enemy;

    private Dictionary<GameObject, BaseEnemy> instantiatedEnemies = new();
    private float counter = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void AddToCounter(float time)
    {
        counter += time;
    }

    public bool IsReady()
    {
        if (counter > spawnDelay)
        {
            counter = 0;
            return true;
        }

        return false;
    }

    public void SpawnEnemy()
    {
        //var exclusionZone = new Bounds(transform.position, new Vector3(2, 2, 2));

        var x = Random.Range(-spawnSize.x, spawnSize.x);
        var y = Random.Range(-spawnSize.y, spawnSize.y);
        var z = Random.Range(-spawnSize.z, spawnSize.z);

        var clonePosition = transform.position + new Vector3(x, y, z);
        var cloneRotation = Quaternion.Euler(0, 0, 0);

        var instantiatedEnemy = Instantiate(enemy, clonePosition, cloneRotation);

        instantiatedEnemies[instantiatedEnemy] = instantiatedEnemy.GetComponent<BaseEnemy>();

        counter = 0;
    }
    public Dictionary<GameObject, BaseEnemy> GetInstantiatedEnemies()
    {
        return instantiatedEnemies;
    }
    public bool Contains(GameObject enemy)
    {
        return instantiatedEnemies.ContainsKey(enemy);
    }
    public void DealDamageToEnemy(GameObject enemy, int damage)
    {
        if (instantiatedEnemies.ContainsKey(enemy))
        {
            var isDestroyed = instantiatedEnemies[enemy].DealDamage(damage);
            if (isDestroyed) 
            { 
                instantiatedEnemies.Remove(enemy);
            }
        }
    }
}
