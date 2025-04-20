using UniRx;
using UnityEngine;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    [Header("weapon")]
    public ReactiveProperty<EWeaponType> weaponSelectRx = new ReactiveProperty<EWeaponType>(EWeaponType.weapon_1);
    public EWeaponType weaponSelect { get => weaponSelectRx.Value; set => weaponSelectRx.Value = value; }
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

        SetAnimatorFire(enemyTarget != null);
    }

    public EnemyObject DetectEnemy()
    {
        return EnemyManager.instance.GetMinDisEnemy();
    }
}
