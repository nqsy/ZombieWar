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

        var newPos = transform.position + direction * GameConfig.instance.speedSoldier;
        var isOutsideX = false;
        var isOutsideZ = false;

        var isOutside = IsOutsideMap(newPos, ref isOutsideX, ref isOutsideZ);

        if (isOutside)
        {
            var x = isOutsideX ? transform.position.x : newPos.x;
            var y = newPos.y;
            var z = isOutsideZ ? transform.position.z : newPos.z;

            transform.position = new Vector3(x, y, z);
        }
        else
        {
            transform.position = newPos;
        }    
    }

    bool IsOutsideMap(Vector3 newPos, ref bool isOutsideX, ref bool isOutsideZ)
    {
        if (newPos.x > GameConfig.instance.maxX || newPos.x < GameConfig.instance.maxX * -1)
        {
            isOutsideX = true;
        }

        if (newPos.x > GameConfig.instance.maxZ || newPos.x < GameConfig.instance.maxZ * -1)
        {
            isOutsideZ = true;
        }

        if(isOutsideX || isOutsideZ)
        {
            return true;
        }    

        return false;
    }
}
