using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    [SerializeField]
    private GameObject wiseTreePrefab;

    private WiseTree wiseTree;

    private TowersDataProvider towersDataProvider;

    private EnemyController enemyController;

    private Dictionary<BaseTower, bool> instantiatedTowers = new();

    private ITowerAimingStrategy aimingStrategy;
    private Dictionary<BaseTower, List<TowerTarget>> targets = new();

    // Start is called before the first frame update
    void Start() 
    {
        towersDataProvider = FindObjectOfType<TowersDataProvider>();
        enemyController = FindObjectOfType<EnemyController>();
        aimingStrategy = new ClosestEnemyTowerAimingStrategy();
        
        PlaceTower(towersDataProvider.GetTowerDatas()[0].Tower, new Vector3(-5f, 0, -5f), transform.rotation);
        PlaceTower(towersDataProvider.GetTowerDatas()[0].Tower, new Vector3(-5f, 0, 5f), transform.rotation);
        PlaceTower(towersDataProvider.GetTowerDatas()[0].Tower, new Vector3(5f, 0, 5f), transform.rotation);
        PlaceTower(towersDataProvider.GetTowerDatas()[0].Tower, new Vector3(5f, 0, -5f), transform.rotation);

        PlaceWiseTree(new Vector3(10f, 0, 0f), transform.rotation);
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
    public TowerTarget GetTarget(BaseTower tower)
    {
        var noTargets = targets.Count == 0;
        var noTower = !targets.TryGetValue(tower, out List<TowerTarget> enemies);
        if (noTargets || noTower) return null;

        TowerTarget target = null;
        
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
    private void PlaceWiseTree(Vector3 position, Quaternion rotation)
    {
        if (wiseTree != null)
        {
            Debug.LogError("Cannout place more than one of wise tree");
            return;
        }

        var tree = Instantiate(wiseTreePrefab, position, rotation);
        wiseTree = tree.GetComponentInChildren<WiseTree>();
    }
    public WiseTree GetWiseTree() 
    { 
        return wiseTree;
    }
}
