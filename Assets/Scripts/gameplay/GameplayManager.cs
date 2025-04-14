using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyObject;

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
        EnemyData enemyData = new EnemyData();
        enemyData.maxHp = 100;

        EnemyManager.instance.SpawnEnemy(enemyData);
    }
}
