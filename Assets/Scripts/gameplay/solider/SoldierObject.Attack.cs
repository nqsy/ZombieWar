using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletObject;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    [SerializeField] float durationFire = 0.1f;
    Cooldown cdFire;

    void InitAttack()
    {
        cdFire = new Cooldown(durationFire);
    }

    void UpdateAttack()
    {
        if (cdFire.IsFinishing)
        {
            var enemy = DetectEnemy();
            if (enemy != null)
            {
                BulletData bulletData = new BulletData();
                bulletData.speed = 0.3f;
                bulletData.dmg = 10;
                bulletData.target = enemy;
                bulletData.rangeMove = 50;

                BulletManager.instance.SpawnBullet(transform.position, bulletData);

                cdFire.Restart(durationFire);
            }
        }
        else
        {
            cdFire.ReduceCooldown();
        }
    }

    EnemyObject DetectEnemy()
    {
        var minDis = float.MaxValue;
        EnemyObject enemyObject = null;

        foreach(var enemy in soldierDetect.enemyObjects)
        {
            var range = Vector3.SqrMagnitude(enemy.transform.position - transform.position);
            if (range < minDis)
            {
                enemyObject = enemy;
                minDis = range;
            }
        }

        return enemyObject;
    }
}
