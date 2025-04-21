using System.Linq;

public enum EnemyType
{
    Base,
    Golem,
    Summoner,
    Summon
}

public static class ControlledEnemyTypes
{
    private static readonly EnemyType[] types = new []{ EnemyType.Summon };
    public static bool IsControlled(EnemyType type)
    {
        return types.Contains(type);
    }
}