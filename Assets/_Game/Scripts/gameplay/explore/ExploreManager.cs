using System.Collections.Generic;
using UnityEngine;

public class ExploreManager : SingletonBehaviour<ExploreManager>
{
    [SerializeField] PoolObject poolExplore;

    List<ExploreObject> exploreObjects = new List<ExploreObject>();

    public void SpawnExploreBomb(Vector3 posSpawn)
    {
        var exploreObj = poolExplore.SpawnObject("explore-bomb");
        var explore = exploreObj.GetComponent<ExploreObject>();

        explore.OnSpawn();
        explore.transform.position = posSpawn;

        exploreObjects.Add(explore);
    }

    public void DespawnExplore(GameObject gameObject)
    {
        poolExplore.DespawnObject(gameObject);
    }

    public void DespawnAllExplore()
    {
        for (int i = exploreObjects.Count - 1; i >= 0; i--)
        {
            exploreObjects[i].Despawn();
        }

        exploreObjects.Clear();
    }
}
