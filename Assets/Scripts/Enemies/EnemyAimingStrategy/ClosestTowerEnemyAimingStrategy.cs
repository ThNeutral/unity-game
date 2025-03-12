using System.Collections.Generic;
using UnityEngine;

public class ClosestTowerEnemyAimingStrategy : IEnemyAimingStrategy
{
    public Dictionary<BaseEnemy, EnemyTarget> GetTargets(
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
                        targets[enemy] = HandleDefaultIntention(enemy, towers, player, wiseTree);
                        break;
                    }
                case BaseEnemy.EnemyTargetIntention.KeepDistance:
                    {
                        targets[enemy] = HandleKeepDistanceIntention(enemy, towers, player, wiseTree);
                        break;
                    }
            }
        }

        return targets;
    }

    private EnemyTarget HandleDefaultIntention(
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
            target = new EnemyTarget { Position = wiseTree.transform.position, Speed = player.GetSpeed() };
        }
        else if (distanceToPlayer < enemy.GetPlayerAgroRange())
        {
            target = new EnemyTarget { Position = player.transform.position, Speed = Vector3.zero };
        } 
        else if (distanceToClosestTower < enemy.GetTowerAgroRange() && closest != null)
        {
            target = new EnemyTarget { Position = closest.transform.position, Speed = Vector3.zero };
        }
        else
        {
            target = new EnemyTarget { Position = wiseTree.transform.position, Speed = Vector3.zero };
        }

        return target;
    }

    private EnemyTarget HandleKeepDistanceIntention(
        BaseEnemy enemy,
        Dictionary<BaseTower, bool> towers,
        PlayerController player,
        WiseTree wiseTree
        )
    {
        Vector3 enemyPos = enemy.transform.position;
        Vector3 futureEnemyPos = enemy.transform.position + enemy.GetSpeed() * 5 * Time.deltaTime;
        Vector3 escapeDirection = Vector3.zero;

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

        if (closest != null) 
        {
            var isInAttackRange = (enemyPos - closest.transform.position).magnitude < closest.GetShootRadius();
            var willBeInAttackRange = (futureEnemyPos - closest.transform.position).magnitude < closest.GetShootRadius();
            
            if (willBeInAttackRange && !isInAttackRange)
            {
                var directionToTower = enemyPos - closest.transform.position;
                var targetLeft = (directionToTower.normalized + Vector3.Cross(directionToTower, -Vector3.up)).normalized;
                var targetRight = (directionToTower.normalized + Vector3.Cross(directionToTower, -Vector3.up)).normalized;
                
                float distanceToLeft = (targetLeft - closest.transform.position).magnitude;
                float distanceToRight = (targetRight - closest.transform.position).magnitude;

                Vector3 target = distanceToLeft > distanceToRight ? targetLeft : targetRight;

                return new EnemyTarget { Position = target, Speed = Vector3.zero };
            }
            
            //RIP BOZO
            if (willBeInAttackRange && isInAttackRange) 
                return new EnemyTarget { Position = enemy.transform.position, Speed = Vector3.zero };
        }

        return new EnemyTarget { Position = wiseTree.transform.position, Speed = Vector3.zero };
    }
}