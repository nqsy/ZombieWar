using System.Collections.Generic;
using UnityEngine;
using static EnemyObject;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    [SerializeField] PoolObject poolEnemy;

    List<EnemyObject> enemyObjects = new List<EnemyObject>();

    public bool IsCanSpawnEnemy()
    {
        return enemyObjects.Count < 150;
    }    

    public void SpawnEnemy(EnemyData enemyData, Vector3 posSpawn)
    {
        var enemyObj = poolEnemy.SpawnObject("enemy");
        var enemy = enemyObj.GetComponent<EnemyObject>();

        enemy.OnSpawn(enemyData);
        enemy.transform.position = posSpawn;

        enemyObjects.Add(enemy);
    }

    public void DespawnEnemy(EnemyObject enemy)
    {
        enemyObjects.Remove(enemy);
        poolEnemy.DespawnObject(enemy.gameObject);
    }
}
