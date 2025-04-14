using UnityEngine;
using static EnemyObject;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    [SerializeField] Pool poolEnemy;

    public void SpawnEnemy(EnemyData enemyData)
    {
        var enemyObj = poolEnemy.SpawnObject("enemy");
        var enemy = enemyObj.GetComponent<EnemyObject>();

        enemy.OnSpawn(enemyData);
        enemy.transform.position = Vector3.zero;
    }

    public void DespawnEnemy(EnemyObject enemy)
    {
        poolEnemy.DespawnObject(enemy.gameObject);
    }
}
