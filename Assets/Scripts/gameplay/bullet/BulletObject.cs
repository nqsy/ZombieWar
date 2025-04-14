using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    public class BulletData
    {
        public EnemyObject target;
        public float speed;
        public float dmg;
        public float rangeMove;
    }

    BulletData bulletData;
    Vector3 normalized;
    Vector3 posFirst;

    public void OnSpawn(Vector3 posSpawn, BulletData bulletData)
    {
        this.bulletData = bulletData;
        posFirst = posSpawn;

        transform.position = posSpawn;
        normalized = (bulletData.target.transform.position - posSpawn).normalized;
    }

    private void Update()
    {
        UpdateMovement();
        CheckOutRange();
    }

    void UpdateMovement()
    {
        transform.position += normalized * bulletData.speed;
    }

    void CheckOutRange()
    {
        var distance = Vector3.Distance(posFirst, transform.position);

        if (distance > bulletData.rangeMove)
        {
            BulletManager.instance.DespawnBullet(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(ConstTag.ENEMY))
        {
            var enemy = other.GetComponent<EnemyObject>();
            enemy.BeAttack(bulletData.dmg);

            BulletManager.instance.DespawnBullet(this);
        }
    }
}
