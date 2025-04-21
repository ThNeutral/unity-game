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
        PlaceTower(towersDataProvider.GetTowerDatas()[0].Tower, new Vector3(-10f, 0, -3f), transform.rotation);
    }

    public BaseEnemy GetTarget(BaseTower tower)
    {
        return enemyController.GetClosestEnemy(tower.transform.position);
    }

    public (BaseTower tower, float distance) GetClosestTower(Vector3 point)
    {
        BaseTower closestTower = null;
        float closestDistance = float.MaxValue;
        foreach (var tower in instantiatedTowers.Keys)
        {
            float distance = Vector3.Distance(tower.transform.position, point);
            if (distance < closestDistance)
            {
                closestTower = tower;
                closestDistance = distance;
            }
        }
        return (closestTower, closestDistance);    
    }

    public List<BaseTower> GetTowersInSphere(Vector3 center, float radius)
    {
        List<BaseTower> towersInRange = new();

        foreach (var tower in instantiatedTowers.Keys)
        {
            float distance = Vector3.Distance(tower.transform.position, center);
            if (distance <= radius)
            {
                towersInRange.Add(tower);
            }
        }

        return towersInRange;
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
