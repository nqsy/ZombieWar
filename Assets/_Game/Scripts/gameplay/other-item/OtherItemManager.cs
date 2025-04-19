using System.Collections.Generic;
using UnityEngine;

public class OtherItemManager : SingletonBehaviour<OtherItemManager>
{
    [SerializeField] Bomb prefabBomb;

    List<OtherItemObject> others = new List<OtherItemObject>();

    public void SpawnBomb(Vector3 posBomb)
    {
        var bomb = Instantiate(prefabBomb, transform);

        bomb.transform.position = posBomb;
        others.Add(bomb);
    }

    public void DespawnOther(OtherItemObject otherItem)
    {
        others.Remove(otherItem);
    }

    public void DespawnAllOther()
    {
        for (int i = others.Count - 1; i >= 0; i--)
        {
            others[i].Despawn();
        }

        others.Clear();
    }
}
