using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    public void UpdateMovement(float vertical, float horizontal)
    {
        vertical = vertical > 0 ? 1 :
            vertical < 0 ? -1 : 0;

        horizontal = horizontal > 0 ? 1 :
            horizontal < 0 ? -1 : 0;

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

        Rotation(newPos);

        transform.position = newPos;
    }

    void Rotation(Vector3 newPos)
    {
        Vector3 direction = newPos - transform.position;

        if (enemyTarget != null)
        {
            direction = enemyTarget.transform.position - transform.position;
        }

        normalized = direction.normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * GameConfig.instance.rotationSpeedSoldier
            );
        }
    }    
}
