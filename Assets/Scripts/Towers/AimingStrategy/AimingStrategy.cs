using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAimingStrategy
{
    Dictionary<BaseTower, List<BaseEnemy>> GetTargets(Dictionary<BaseTower, bool> towers, Dictionary<BaseEnemy, bool> enemies);
}