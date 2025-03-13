using System.Collections.Generic;

public interface IEnemyAimingStrategy
{
    Dictionary<BaseEnemy, EnemyTarget> GetTargets(
        Dictionary<BaseEnemy, bool> enemies,
        Dictionary<BaseTower, bool> towers,
        PlayerController player,
        WiseTree wiseTree
        );
}