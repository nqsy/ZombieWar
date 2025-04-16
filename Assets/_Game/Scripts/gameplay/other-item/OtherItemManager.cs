using UnityEngine;

public class OtherItemManager : SingletonBehaviour<OtherItemManager>
{
    [SerializeField] Bomb prefabBomb;

    public void SpawnBomb(Vector3 posBomb)
    {
        var bomb = Instantiate(prefabBomb, transform);

        bomb.transform.position = posBomb;
    }
}
