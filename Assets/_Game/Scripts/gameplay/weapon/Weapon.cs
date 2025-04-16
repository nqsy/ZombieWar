using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletObject;

public class Weapon : MonoBehaviour
{
    [SerializeField] float durationFire = 0.1f;
    [SerializeField] EWeaponType weaponType;
    [SerializeField] Transform firePos;
    [SerializeField] List<ParticleSystem> muzzles;
    Cooldown cdFire;

    private void Start()
    {
        cdFire = new Cooldown(durationFire);
    }

    private void Update()
    {
        UpdateAttack();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    void UpdateAttack()
    {
        if (cdFire.IsFinishing)
        {
            Fire();
        }
        else
        {
            cdFire.ReduceCooldown();
        }
    }

    void Fire()
    {
        var enemy = SoldierObject.instance.enemyTarget;
        if (enemy != null)
        {
            bool weapon1 = weaponType == EWeaponType.weapon_1;

            BulletData bulletData = GetBulletData();
            bulletData.target = enemy;
            bulletData.normalized = SoldierObject.instance.normalized;

            BulletManager.instance.SpawnBullet(firePos.position, bulletData);

            cdFire.Restart(weapon1 ? durationFire : durationFire / 2);

            SoundManager.instance.PlaySound(weaponType.GetSoundType());
            ActiveMuzzle();
        }
    }

    void ActiveMuzzle()
    {
        foreach (var muzzle in muzzles)
        {
            muzzle.Emit(1);
        }
    }

    BulletData GetBulletData()
    {
        bool weapon1 = weaponType == EWeaponType.weapon_1;

        BulletData bulletData = new BulletData();
        bulletData.speed = weapon1 ? 0.3f : 0.5f;
        bulletData.dmg = weapon1 ? 30 : 10;
        bulletData.rangeMove = weapon1 ? 50 : 40;

        return bulletData;
    }
}
