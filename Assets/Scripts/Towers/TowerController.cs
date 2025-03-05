using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    private TowersDataProvider towersDataProvider;

    private EnemyController enemyController;

    private Dictionary<BaseTower, bool> instantiatedTowers = new();

    private ITowerAimingStrategy aimingStrategy;
    private Dictionary<BaseTower, List<BaseEnemy>> targets = new();

    // Start is called before the first frame update
    void Start() 
    {
        towersDataProvider = FindObjectOfType<TowersDataProvider>();
        enemyController = FindObjectOfType<EnemyController>();
        aimingStrategy = new ClosestEnemyTowerAimingStrategy();
        PlaceTower(towersDataProvider.GetTowerDatas()[0].Tower, transform.position + new Vector3(-7.5f, 0, -7.5f), transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Count == 0)
        {
            targets = aimingStrategy.GetTargets(instantiatedTowers, enemyController.GetInstantiatedEnemies());
        }
    }
    public Dictionary<BaseTower, bool> GetInstantiatedTowers()
    {
        return instantiatedTowers;
    }
    public BaseEnemy GetTarget(BaseTower tower)
    {
        var noTargets = targets.Count == 0;
        var noTower = !targets.TryGetValue(tower, out List<BaseEnemy> enemies);
        if (noTargets || noTower) return null;
        
        BaseEnemy target = null;
        
        var noTargetsForTower = enemies.Count == 0;

        if (!noTargetsForTower)
        {
            int index = Random.Range(0, enemies.Count);
            target = enemies[index];
            enemies.RemoveAt(index);
        }

        if (enemies.Count == 0)
        {
            targets.Remove(tower);
        }
        
        return target;
    }
    public GameObject PlaceTower(GameObject towerPrefab, Vector3 position, Quaternion rotation)
    {
        var tower = Instantiate(towerPrefab, position, rotation);
        var behaviour = tower.GetComponentInChildren<BaseTower>();
        behaviour.SetEnemyController(enemyController);
        behaviour.SetTowerController(this);
        instantiatedTowers[behaviour] = true;
        return tower;
    }
}
