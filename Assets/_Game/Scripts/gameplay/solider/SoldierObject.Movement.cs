using UniRx;
using UnityEngine;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    public void UpdateMovement(float vertical, float horizontal)
    {
        Vector3 direction = Vector3.forward * vertical + Vector3.right * horizontal;
        if(direction == Vector3.zero)
        {
            SetAnimatorMove(false);
        }
        else
        {
            SetAnimatorMove(true);
        }

        transform.position += direction * GameConfig.instance.speedSoldier;
    }
}
