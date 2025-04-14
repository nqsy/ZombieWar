using UnityEngine;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    [SerializeField] Pool poolEnemy;

    public void SpawnEnemy()
    {
        var enemy = poolEnemy.SpawnObject("enemy");
        enemy.transform.position = Vector3.zero;
    }
}
