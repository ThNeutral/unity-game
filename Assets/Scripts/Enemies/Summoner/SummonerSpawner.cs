using UnityEngine;

public class SummonerSpawner : BaseSpawner
{
    [SerializeField]
    private Summoner summoner;

    private void Start()
    {
        Initialize();
        enemyController.RegisterSpawner(this);
    }

    private void Update()
    {
        if (instantiatedEnemies.Count >= maxNumberOfSummons) return;

        spawnCounter += Time.deltaTime;
        while (spawnCounter > spawnDelay)
        {
            SpawnEnemy(true);
            spawnCounter -= spawnDelay;
        }
    }

    private void OnDestroy()
    {
        enemyController.MoveEnemiesToDefaultSpawner(this);
        enemyController.UnregisterSpawner(this);
    }

    public void SetTarget(EnemyTarget target)
    {
        foreach (var enemy in instantiatedEnemies.Keys)
        {
            enemy.SetTarget(target);
        }
    }
}