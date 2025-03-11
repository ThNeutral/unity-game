using UnityEngine;

public class SummonerSpanwer : BaseSpawner
{
    [SerializeField]
    private GameObject summoner;

    private new void Start()
    {
        base.Start();
        enemyController.RegisterSpawner(this);
    }

    private void OnDestroy()
    {
        enemyController.MoveEnemiesToDefaultSpawner(this);
        enemyController.UnregisterSpawner(this);
    }
}