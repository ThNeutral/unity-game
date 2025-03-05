using System.Collections.Generic;
using UnityEngine;

public class ClosestTowerEnemyAimingStrategy : IEnemyAimingStrategy
{
    public Dictionary<BaseEnemy, BaseTower> GetTargets(Dictionary<BaseEnemy, bool> enemies, Dictionary<BaseTower, bool> towers)
    {
        var targets = new Dictionary<BaseEnemy, BaseTower>();

        foreach (var enemy in enemies.Keys)
        {
            BaseTower closest = null;
            float minDistance = float.MaxValue;

            foreach (var tower in towers.Keys)
            {
                float currentDistance = Vector3.Distance(enemy.transform.position, tower.transform.position);
                if (currentDistance < minDistance)
                {
                    closest = tower;
                    minDistance = currentDistance;
                }
            }

            targets[enemy] = closest;
        }

        return targets;
    }
}