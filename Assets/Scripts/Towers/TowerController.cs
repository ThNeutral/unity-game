using System;
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

    private List<GameObject> instantiatedTowers = new();
    // Start is called before the first frame update
    void Start()
    {
        var tower = PlaceTower();

        enemyController = FindObjectOfType<EnemyController>();
        tower.GetComponent<BaseTower>().SetEnemyController(enemyController);
    }

    // Update is called once per frame
    void Update()
    {
        var enemies = enemyController.GetInstantiatedEnemies().ToList();

        foreach (var tower in instantiatedTowers) 
        {
            var behaviour = tower.GetComponent<BaseTower>();
            behaviour.UpdateCounter(Time.deltaTime);

            if (enemies.Count == 0) continue;
        
            if (behaviour.IsReady())
            {
                behaviour.ShootAt(enemies[UnityEngine.Random.Range(0, enemies.Count - 1)].Key);
            }
        }
    }
    public GameObject PlaceTower()
    {
        var tower = Instantiate(towerPrefab);
        instantiatedTowers.Add(tower);
        return tower;
    }
    public GameObject PlaceTower(Vector3 position, Quaternion rotation)
    {
        var tower = Instantiate(towerPrefab, position, rotation);
        instantiatedTowers.Add(tower);
        return tower;
    }
    public GameObject PlaceTower(GameObject towerPrefab, Vector3 position, Quaternion rotation)
    {
        var tower = Instantiate(towerPrefab, position, rotation);
        instantiatedTowers.Add(tower);
        return tower;
    }
}
