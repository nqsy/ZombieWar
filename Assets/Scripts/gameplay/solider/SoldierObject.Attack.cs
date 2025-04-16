using UniRx;
using UnityEngine;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    [Header("weapon")]
    public ReactiveProperty<EWeaponType> weaponSelectRx = new ReactiveProperty<EWeaponType>(EWeaponType.weapon_1);
    [SerializeField] Weapon weapon_1;
    [SerializeField] Weapon weapon_2;

    public EnemyObject enemyTarget;

    void InitWeapon()
    {
        weaponSelectRx
            .Subscribe(val =>
            {
                weapon_1.SetActive(val == EWeaponType.weapon_1);
                weapon_2.SetActive(val == EWeaponType.weapon_2);
            }).AddTo(this);
    }

    void UpdateWeapon()
    {
        enemyTarget = DetectEnemy();
    }

    public EnemyObject DetectEnemy()
    {
        var minDis = float.MaxValue;
        EnemyObject enemyObject = null;

        foreach (var enemy in soldierDetect.enemyObjects)
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
