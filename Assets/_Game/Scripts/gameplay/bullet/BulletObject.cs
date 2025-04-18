using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    [System.Serializable]
    public class BulletData
    {
        public EnemyObject target;
        public float speed;
        public float dmg;
        public float rangeMove;
        public Vector3 normalized;
    }

    [SerializeField] BulletData bulletData;
    Vector3 posFirst;

    public void OnSpawn(Vector3 posSpawn, BulletData bulletData)
    {
        this.bulletData = bulletData;
        posFirst = posSpawn;

        transform.position = posSpawn;
    }

    private void Update()
    {
        UpdateMovement();
        CheckOutRange();
    }

    void UpdateMovement()
    {
        transform.position += bulletData.normalized * bulletData.speed;
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
            var enemy = other.GetComponent<EnemyCollider>().EnemyObject;
            if (!enemy.isAlive)
                return;

            enemy.BeAttack(bulletData.dmg);

            BulletManager.instance.DespawnBullet(this);
        }
    }
}
