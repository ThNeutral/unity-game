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

    // Start is called before the first frame update
    void Start() 
    {
        towersDataProvider = FindObjectOfType<TowersDataProvider>();
        enemyController = FindObjectOfType<EnemyController>();
        PlaceTower(towersDataProvider.GetTowerDatas()[0].Tower, new Vector3(-20f, 0, -10f), transform.rotation);
    }

    public BaseEnemy GetTarget(BaseTower tower)
    {
        return enemyController.GetClosestEnemy(tower.transform.position);
    }

    public Dictionary<BaseTower, bool> GetInstantiatedTowers()
    {
        return instantiatedTowers;
    }

    public GameObject PlaceTower(GameObject towerPrefab, Vector3 position, Quaternion rotation)
    {
        var tower = Instantiate(towerPrefab, position, rotation);
        var behaviour = tower.GetComponentInChildren<BaseTower>();
        instantiatedTowers[behaviour] = true;
        return tower;
    }
}
