using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : BaseEnemy
{
    [SerializeField]
    private SummonerSpawner spawner;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        HandleTarget();
        HandleTargetForSummons();
        HandleMove();
    }

    private void HandleTargetForSummons()
    {
        if (enemyController.IsValidTarget(this, target))
        {
            spawner.SetTarget(target);
        }
    }
}
