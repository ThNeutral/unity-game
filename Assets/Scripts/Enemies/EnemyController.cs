using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private float spawnDelay = 0.2f;

    [SerializeField]
    private Vector3 leftTopSpawnLimit;

    [SerializeField]
    private Vector3 bottomRightSpawnLimit;

    private Dictionary<GameObject, bool> instantiatedEnemies = new();

    private float counter = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counter > spawnDelay)
        {
            var x = Random.Range(leftTopSpawnLimit.x, bottomRightSpawnLimit.x);
            var y = Random.Range(leftTopSpawnLimit.y, bottomRightSpawnLimit.y);
            var z = Random.Range(leftTopSpawnLimit.z, bottomRightSpawnLimit.z);

            var clonePosition = new Vector3(x, y, z);
            var cloneRotation = Quaternion.Euler(0, 0, 0);

            var instantiatedEnemy = Instantiate(enemy, clonePosition, cloneRotation);

            instantiatedEnemies[instantiatedEnemy] = true;

            counter = 0;
        } 
        else
        {
            counter += Time.deltaTime;
        }
    }

    public Dictionary<GameObject, bool> GetInstantiatedEnemies()
    {
        return instantiatedEnemies;
    }

    public void DealDamageTo(GameObject enemy, int damage)
    {
        if (instantiatedEnemies.ContainsKey(enemy))
        {
            var isDead = enemy.GetComponent<BaseEnemy>().DealDamage(damage);
            if (isDead) {
                instantiatedEnemies.Remove(enemy);
            }
        }
    }
}
