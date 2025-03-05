using System.Collections.Generic;

public interface IEnemyAimingStrategy
{
    Dictionary<BaseEnemy, BaseTower> GetTargets(Dictionary<BaseEnemy, bool> enemies, Dictionary<BaseTower, bool> towers);
}