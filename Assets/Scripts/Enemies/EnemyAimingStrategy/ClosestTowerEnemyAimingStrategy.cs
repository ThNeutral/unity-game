using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ClosestTowerEnemyAimingStrategy
{
    public Dictionary<BaseEnemy, EnemyTarget> GetTargets(
        NavigationProvider navigationProvider,
        Dictionary<BaseEnemy, bool> enemies,
        Dictionary<BaseTower, bool> towers,
        PlayerController player,
        WiseTree wiseTree
        )
    {
        var targets = new Dictionary<BaseEnemy, EnemyTarget>();

        foreach (var enemy in enemies.Keys)
        {
            switch (enemy.GetTargetIntention())
            {
                case BaseEnemy.EnemyTargetIntention.Default:
                    {
                        targets[enemy] = HandleDefaultIntention(navigationProvider, enemy, towers, player, wiseTree);
                        break;
                    }
                case BaseEnemy.EnemyTargetIntention.KeepDistance:
                    {
                        targets[enemy] = HandleKeepDistanceIntention(navigationProvider, enemy, towers, player, wiseTree);
                        break;
                    }
            }
        }

        return targets;
    }

    private EnemyTarget HandleDefaultIntention(
        NavigationProvider navigationProvider,
        BaseEnemy enemy,
        Dictionary<BaseTower, bool> towers,
        PlayerController player,
        WiseTree wiseTree
        )
    {
        BaseTower closest = null;
        float distanceToClosestTower = float.MaxValue;

        foreach (var tower in towers.Keys)
        {
            float currentDistance = Vector3.Distance(enemy.transform.position, tower.transform.position);
            if (currentDistance < distanceToClosestTower)
            {
                closest = tower;
                distanceToClosestTower = currentDistance;
            }
        }

        EnemyTarget target = null;
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);
        float distanceToWiseTree = Vector3.Distance(enemy.transform.position, wiseTree.transform.position);

        if (distanceToWiseTree < enemy.GetTreeAgroRange())
        {
            target = new EnemyTarget { Position = wiseTree.transform.position, Speed = Vector3.zero };
        }
        else if (distanceToPlayer < enemy.GetPlayerAgroRange())
        {
            target = new EnemyTarget { Position = player.transform.position, Speed = Vector3.zero };
        } 
        else if (closest != null && distanceToClosestTower < enemy.GetTowerAgroRange())
        {
            target = new EnemyTarget { Position = closest.transform.position, Speed = Vector3.zero };
        }
        else
        {
            target = new EnemyTarget { Position = wiseTree.transform.position, Speed = Vector3.zero };
        }

        return navigationProvider.GetNextPathNode(enemy, null, target);
    }

    private EnemyTarget HandleKeepDistanceIntention(
        NavigationProvider navigationProvider,
        BaseEnemy enemy,
        Dictionary<BaseTower, bool> towers,
        PlayerController player,
        WiseTree wiseTree)
    {
        Vector3 enemyPos = enemy.transform.position;
        Vector3 futureEnemyPos = enemyPos + enemy.GetSpeedDirection() * enemy.GetTowerAvoidanceDistance();

        BaseTower closestTower = null;
        float closestTowerDistance = float.MaxValue;

        foreach (var tower in towers.Keys)
        {
            float currentDistance = Vector3.Distance(enemyPos, tower.transform.position);
            if (currentDistance < closestTowerDistance)
            {
                closestTower = tower;
                closestTowerDistance = currentDistance;
            }
        }

        
        return navigationProvider.GetNextPathNode(enemy, towers, new EnemyTarget { Position = wiseTree.transform.position });
    }
}