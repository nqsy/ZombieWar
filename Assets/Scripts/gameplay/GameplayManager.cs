using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : SingletonBehaviour<GameplayManager>
{
    Cooldown cdSpawnEnemy;

    private void Start()
    {
        cdSpawnEnemy = new Cooldown(10);
        cdSpawnEnemy.SetRemain(0);
    }

    private void Update()
    {
        cdSpawnEnemy.ReduceCooldown();

        if (cdSpawnEnemy.IsFinishing)
        {
            SpawnEnemy();
            cdSpawnEnemy.Restart();
        }
    }

    void SpawnEnemy()
    {
        EnemyManager.instance.SpawnEnemy();
    }
}
