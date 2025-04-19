using System.Collections.Generic;
using UnityEngine;
using static EnemyObject;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    [SerializeField] PoolObject poolEnemy;

    List<EnemyObject> enemyObjects = new List<EnemyObject>();

    public bool IsCanSpawnEnemy()
    {
        return enemyObjects.Count < GameplayManager.instance.levelMap.maxEnemyOnMap;
    }

    public void SpawnNormalEnemy(EnemyData enemyData, Vector3 posSpawn)
    {
        var random = Random.Range(1, 3);

        SpawnEnemy(enemyData, posSpawn, $"enemy_{random}");
    }

    public void SpawnBigEnemy(EnemyData enemyData, Vector3 posSpawn)
    {
        SpawnEnemy(enemyData, posSpawn, "big_enemy");
    }    

    public void SpawnEnemy(EnemyData enemyData, Vector3 posSpawn, string namePrefab)
    {
        var enemyObj = poolEnemy.SpawnObject(namePrefab);
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

    public void DespawnAllEnemy()
    {
        for (int i = enemyObjects.Count - 1; i >= 0; i--)
        {
            enemyObjects[i].Despawn();
        }

        enemyObjects.Clear();
    }
}
