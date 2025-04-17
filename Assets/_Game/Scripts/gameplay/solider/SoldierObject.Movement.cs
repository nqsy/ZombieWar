using UniRx;
using UnityEngine;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    public void UpdateMovement(float vertical, float horizontal)
    {
        Vector3 direction = Vector3.forward * vertical + Vector3.right * horizontal;

        transform.position += direction * GameConfig.instance.speedSoldier;
    }
}
