using UnityEngine;

public class OtherItemObject : MonoBehaviour
{
    public void Despawn()
    {
        OtherItemManager.instance.DespawnOther(this);
        Destroy(gameObject);
    }
}
