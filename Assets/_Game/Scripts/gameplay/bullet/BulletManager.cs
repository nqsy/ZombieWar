using UnityEngine;
using static BulletObject;

public class BulletManager : SingletonBehaviour<BulletManager>
{
    [SerializeField] PoolObject bulletPool;

    public void SpawnBullet(Vector3 posSpawn, BulletData bulletData)
    {
        var bulletObj = bulletPool.SpawnObject("bullet-1");
        var bullet = bulletObj.GetComponent<BulletObject>();

        bullet.OnSpawn(posSpawn, bulletData);
    }

    public void DespawnBullet(BulletObject bulletObject)
    {
        bulletPool.DespawnObject(bulletObject.gameObject);
    }
}
