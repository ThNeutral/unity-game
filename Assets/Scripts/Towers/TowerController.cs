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

    private Dictionary<GameObject, BaseTower> instantiatedTowers = new();
    // Start is called before the first frame update
    void Start()
    {
        enemyController = FindObjectOfType<EnemyController>();;
    }

    // Update is called once per frame
    void Update()
    {
        var enemies = enemyController.GetInstantiatedEnemies().ToList();

        foreach (var tower in instantiatedTowers.Keys) 
        {
            var behaviour = instantiatedTowers[tower];
            behaviour.AddToCounter(Time.deltaTime);

            if (enemies.Count == 0) continue;
        
            if (behaviour.IsReady())
            {
                behaviour.ShootAt(enemies[UnityEngine.Random.Range(0, enemies.Count - 1)].Key);
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
