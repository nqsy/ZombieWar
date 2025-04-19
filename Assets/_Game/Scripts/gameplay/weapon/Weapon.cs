using System.Collections.Generic;
using UnityEngine;
using static BulletObject;

public class Weapon : MonoBehaviour
{
    [System.Serializable]
    public class WeaponData
    {
        public EWeaponType weaponType;
        public float durationFire;
        public float speedBullet;
        public float dmg;
        public float rangeMoveBullet;
    }

    [SerializeField] EWeaponType weaponType;
    [SerializeField] Transform firePos;
    [SerializeField] List<ParticleSystem> muzzles;
    Cooldown cdFire;

    WeaponData weaponData;

    private void Start()
    {
        weaponData = GameConfig.instance.GetWeaponData(weaponType);
        cdFire = new Cooldown(weaponData.durationFire);
    }

    private void Update()
    {
        if (SoldierObject.instance.isDeath) return;

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
            BulletData bulletData = GetBulletData();
            bulletData.target = enemy;
            bulletData.normalized = SoldierObject.instance.normalized;

            BulletManager.instance.SpawnBullet(firePos.position, bulletData);

            cdFire.Restart(duration: weaponData.durationFire);

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
        BulletData bulletData = new BulletData();
        bulletData.speed = weaponData.speedBullet;
        bulletData.dmg = weaponData.dmg;
        bulletData.rangeMove = weaponData.rangeMoveBullet;

        return bulletData;
    }
}
