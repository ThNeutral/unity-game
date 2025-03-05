using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClosestEnemyTowerAimingStrategy : ITowerAimingStrategy
{
    public Dictionary<BaseTower, List<BaseEnemy>> GetTargets(Dictionary<BaseTower, bool> towers, Dictionary<BaseEnemy, bool> enemies)
    {
        Dictionary<BaseTower, List<BaseEnemy>> towerAssignments = new();

        foreach (var tower in towers.Keys)
        {
            towerAssignments[tower] = new List<BaseEnemy>();
        }

        foreach (var enemy in enemies.Keys)
        {
            BaseTower bestTower = null;
            float bestDistance = float.MaxValue;

            foreach (var tower in towers.Keys)
            {
                float distance = Vector3.Distance(tower.transform.position, enemy.transform.position);

                if (distance < bestDistance)
                {
                    bestTower = tower;
                    bestDistance = distance;
                }
            }

            if (bestTower != null)
            {
                towerAssignments[bestTower].Add(enemy);
            }
        }

        return towerAssignments;
    }
}
