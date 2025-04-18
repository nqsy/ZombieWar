using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
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
        bool isNew;
        return SpawnObject(name, out isNew);
    }

    public GameObject SpawnObject(string name, out bool isNew)
    {
        var prefab = poolData.Find(e => e.name == name);
        var prefabName = prefab.name;

        if (!dictCache.ContainsKey(prefabName))
        {
            dictCache.Add(prefabName, new List<GameObject>());
        }

        var objActive = dictCache[prefabName].Find(e => !e.activeSelf);

        if (objActive != null)
        {
            objActive.SetActive(true);

            isNew = false;
            return objActive;
        }
        else
        {
            var obj = Instantiate(prefab.prefab, transform);

            dictCache[prefabName].Add(obj);

            isNew = true;
            return obj;
        }
    }  
    
    public void DespawnObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
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
