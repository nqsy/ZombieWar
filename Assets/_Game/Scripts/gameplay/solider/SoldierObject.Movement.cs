using UniRx;
using Unity.VisualScripting;
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

        var newPos = transform.position + direction * GameConfig.instance.speedSoldier;

        newPos.x = Mathf.Clamp(newPos.x, -GameConfig.instance.maxX, GameConfig.instance.maxX);
        newPos.z = Mathf.Clamp(newPos.z, -GameConfig.instance.maxZ, GameConfig.instance.maxZ);
        transform.position = newPos;
    }
}
