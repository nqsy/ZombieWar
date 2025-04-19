using System.Collections.Generic;
using UnityEngine;
using static EnemyObject;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    const int MAX_GROUP = 8;
    [SerializeField] PoolObject poolEnemy;

    [SerializeField] List<EnemyObject> enemyObjects = new List<EnemyObject>();

    public int groupActive = 1;

    int groupEnemy = 1;

    private void Update()
    {
        if(groupActive + 1 > MAX_GROUP)
        {
            groupActive = 1;
        }    
        else
        {
            groupActive++;
        }    
    }

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
        bool isNew;
        var enemyObj = poolEnemy.SpawnObject(namePrefab, out isNew);
        var enemy = enemyObj.GetComponent<EnemyObject>();

        enemy.OnSpawn(enemyData);
        enemy.transform.position = posSpawn;

        if(isNew)
        {
            enemy.groupActiveEnemy = GetGroupActiveEnemy();
        }    

        enemyObjects.Add(enemy);
    }

    int GetGroupActiveEnemy()
    {
        if (groupEnemy + 1 > MAX_GROUP)
        {
            groupEnemy = 1;
        }
        else
        {
            groupEnemy++;
        }
        return groupEnemy;
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
