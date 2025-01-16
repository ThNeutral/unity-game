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
    // Start is called before the first frame update
    void Start() 
    {
        enemyController = FindObjectOfType<EnemyController>();
        PlaceTower(transform.position + new Vector3(-10, 0, -10), transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var kvp in instantiatedTowers) 
        {
            var behaviour = kvp.Value;
            behaviour.AddToCounter(Time.deltaTime);

            if (behaviour.IsReady())
            {
                var enemy = enemyController.GetClosestEnemy(kvp.Key.transform.position);
                if (enemy != null)
                {
                    behaviour.ShootAt(enemy);
                }
            }
        }
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
        instantiatedTowers[tower] = behaviour;
        return tower;
    }
}
