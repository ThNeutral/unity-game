using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class GolemSpawner : BaseSpawner
{
    private void Start()
    {
        SpawnEnemy();
    }

    private void Update()
    {
    }
}
