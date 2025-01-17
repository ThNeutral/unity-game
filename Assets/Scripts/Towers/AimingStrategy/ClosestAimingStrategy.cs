using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestAimingStrategy : IAimingStrategy
{
    public Dictionary<BaseTower, List<BaseEnemy>> GetTargets(Dictionary<GameObject, BaseTower> towers, Dictionary<GameObject, BaseEnemy> enemies)
    {
        Dictionary<BaseTower, List<BaseEnemy>> towerAssignments = new();

        foreach (var tower in towers.Values)
        {
            towerAssignments[tower] = new List<BaseEnemy>();
        }

        foreach (var enemy in enemies.Values)
        {
            BaseTower bestTower = null;
            float bestDistance = float.MaxValue;

            foreach (var tower in towers.Values)
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
