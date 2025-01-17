using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    private EnemyController enemyController;

    private Dictionary<GameObject, BaseTower> instantiatedTowers = new();

    private IAimingStrategy aimingStrategy;
    private Dictionary<BaseTower, List<BaseEnemy>> targets = new();
    // Start is called before the first frame update
    void Start() 
    {
        enemyController = FindObjectOfType<EnemyController>();
        aimingStrategy = new ClosestAimingStrategy();
        PlaceTower(transform.position + new Vector3(-10, 0, -8), transform.rotation);
        PlaceTower(transform.position + new Vector3(-8, 0, -10), transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Count == 0)
        {
            targets = aimingStrategy.GetTargets(instantiatedTowers, enemyController.GetInstantiatedEnemies());
        }
    }
    public BaseEnemy GetTarget(BaseTower tower)
    {
        BaseEnemy target = null;
        
        var noTargets = targets.Count == 0;
        var noTower = !targets.TryGetValue(tower, out List<BaseEnemy> enemies);
        if (noTargets || noTower) return target;
        
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
    public GameObject PlaceTower(Vector3 position, Quaternion rotation)
    {
        return PlaceTower(towerPrefab, position, rotation);
    }

    public GameObject PlaceTower(GameObject towerPrefab, Vector3 position, Quaternion rotation)
    {
        var tower = Instantiate(towerPrefab, position, rotation);
        var behaviour = tower.GetComponentInChildren<BaseTower>();
        behaviour.SetEnemyController(enemyController);
        behaviour.SetTowerController(this);
        instantiatedTowers[tower] = behaviour;
        return tower;
    }
}
