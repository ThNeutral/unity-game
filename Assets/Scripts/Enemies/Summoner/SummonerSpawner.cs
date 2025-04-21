using System.Drawing;
using UnityEngine;

public class SummonerSpawner : BaseSpawner 
{
    private new void Start()
    {
        base.Start();
        isControlled = true;
    }

    public int RequestSummon()
    {
        int diff = maxNumberOfSpawns - CurrentNumberOfSpawns;
        for (int i = diff; i > 0; i--) SpawnEnemy();
        return diff;
    }

    public void SetTarget(MonoBehaviour target)
    {
        foreach (var enemy in instantiatedEnemies.Keys) 
        { 
            try
            {
                ((Summon)enemy).SetTarget(target);
            }
            catch 
            {
                Debug.LogError("Failed to set target to summon");
            }
        }
    }

    public void SetPoint(NavigationPoint point)
    {
        foreach (var enemy in instantiatedEnemies.Keys)
        {
            try
            {
                ((Summon)enemy).SetPoint(point);
            }
            catch
            {
                Debug.LogError("Failed to set target to summon");
            }
        }
    }

    public void Freeze()
    {
        foreach (var enemy in instantiatedEnemies.Keys)
        {
            try
            {
                ((Summon)enemy).Freeze();
            }
            catch
            {
                Debug.LogError("Failed to set target to summon");
            }
        }
    }
}