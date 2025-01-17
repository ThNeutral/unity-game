using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAimingStrategy
{
    Dictionary<BaseTower, List<BaseEnemy>> GetTargets(Dictionary<GameObject, BaseTower> towers, Dictionary<GameObject, BaseEnemy> enemies);
}