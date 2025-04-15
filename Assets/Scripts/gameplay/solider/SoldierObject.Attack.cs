using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static BulletObject;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    public ReactiveProperty<int> weaponSelectRx = new ReactiveProperty<int>(1);

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
                bool weapon1 = weaponSelectRx.Value == 1;

                BulletData bulletData = GetBulletData();
                bulletData.target = enemy;

                BulletManager.instance.SpawnBullet(transform.position, bulletData);

                cdFire.Restart(weapon1 ? durationFire : durationFire / 2);
            }
        }
        else
        {
            cdFire.ReduceCooldown();
        }
    }

    BulletData GetBulletData()
    {
        bool weapon1 = weaponSelectRx.Value == 1;

        BulletData bulletData = new BulletData();
        bulletData.speed = weapon1 ? 0.3f : 0.5f;
        bulletData.dmg =  weapon1 ? 30 : 10;
        bulletData.rangeMove = weapon1 ? 50 : 40;

        return bulletData;
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
