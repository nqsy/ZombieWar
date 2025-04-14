using UnityEngine;
using static EnemyObject;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    [SerializeField] PoolObject poolEnemy;

    public void SpawnEnemy(EnemyData enemyData, Vector3 posSpawn)
    {
        var enemyObj = poolEnemy.SpawnObject("enemy");
        var enemy = enemyObj.GetComponent<EnemyObject>();

        enemy.OnSpawn(enemyData);
        enemy.transform.position = posSpawn;
    }

    public void DespawnEnemy(EnemyObject enemy)
    {
        poolEnemy.DespawnObject(enemy.gameObject);
    }
}
