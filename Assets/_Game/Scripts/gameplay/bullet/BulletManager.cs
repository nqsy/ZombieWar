using System.Collections.Generic;
using UnityEngine;
using static BulletObject;

public class BulletManager : SingletonBehaviour<BulletManager>
{
    [SerializeField] PoolObject bulletPool;

    List<BulletObject> bulletObjects = new List<BulletObject>();

    public void SpawnBullet(Vector3 posSpawn, BulletData bulletData)
    {
        var bulletObj = bulletPool.SpawnObject("bullet-1");
        var bullet = bulletObj.GetComponent<BulletObject>();

        bullet.OnSpawn(posSpawn, bulletData);

        bulletObjects.Add(bullet);
    }

    public void DespawnBullet(BulletObject bulletObject)
    {
        bulletPool.DespawnObject(bulletObject.gameObject);
    }

    public void DespawnAllBullet()
    {
        for (int i = bulletObjects.Count - 1; i >= 0; i--)
        {
            bulletObjects[i].Despawn();
        }

        bulletObjects.Clear();
    }
}
