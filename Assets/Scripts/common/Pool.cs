using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [System.Serializable]
    public class PoolData
    {
        public string name;
        public GameObject prefab;
    }

    [SerializeField] List<PoolData> poolData;

    [SerializeField] Dictionary<string, List<GameObject>> dictCache = new Dictionary<string, List<GameObject>>();

    public GameObject SpawnObject(string name)
    {
        var prefab = poolData.Find(e => e.name == name);
        var prefabName = prefab.name;

        if (dictCache.ContainsKey(prefabName) && dictCache[prefabName].Count > 0)
        {
            var obj = dictCache[prefabName][0];
            obj.SetActive(true);

            dictCache[prefabName].RemoveAt(0);
            return obj;
        }
        else
        {
            var obj = Instantiate(prefab.prefab, transform);

            return obj;
        }
    }  
    
    public void DespawnObject(GameObject gameObject)
    {
        var prefabName = gameObject.name;

        if (!dictCache.ContainsKey(prefabName))
        {
            dictCache.Add(prefabName, new List<GameObject>());
        }

        dictCache[prefabName] = new List<GameObject>();
    }    

    public void DespawnAllObject()
    {
        foreach(Transform child in  transform)
        {
            if(child.gameObject.activeSelf)
            {
                DespawnObject(child.gameObject);
            }    
        }    
    }   
}
