using UnityEngine;

public class ExploreManager : SingletonBehaviour<ExploreManager>
{
    [SerializeField] PoolObject poolExplore;

    public void SpawnExploreBomb(Vector3 posSpawn)
    {
        var enemyObj = poolExplore.SpawnObject("explore-bomb");
        var enemy = enemyObj.GetComponent<ExploreObject>();

        enemy.OnSpawn();
        enemy.transform.position = posSpawn;
    }

    public void DespawnExplore(GameObject gameObject)
    {
        poolExplore.DespawnObject(gameObject);
    }    
}
