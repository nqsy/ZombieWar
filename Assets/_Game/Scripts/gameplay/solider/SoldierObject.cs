using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    [Header("other")]
    [SerializeField] SoldierDetect soldierDetect;
    [SerializeField] Animator animator;
    [SerializeField] List<ParticleSystem> particleBloods;

    private readonly Vector3 defaultDirection = new Vector3(0, 1, 0);

    [Header("for debug")]
    public ReactiveProperty<float> hpRx = new ReactiveProperty<float>();
    public Vector3 normalized;

    private void Start()
    {
        InitWeapon();
        hpRx.Value = GameConfig.instance.maxHpSoldier;
    }

    private void Update()
    {
        UpdateWeapon();

        if (enemyTarget != null)
        {
            Vector3 direction = enemyTarget.transform.position - transform.position;
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

    public void SpawnBomb()
    {
        OtherItemManager.instance.SpawnBomb(transform.position);
    }

    public void BeAttack(float dmg)
    {
        hpRx.Value -= dmg;
        ActiveBlood();
    }

    void ActiveBlood()
    {
        foreach (var particle in particleBloods)
        {
            particle.Emit(5);
        }
    }

    public void SetAnimatorFire(bool val)
    {
        animator.SetBool("isFire", val);
    }    

    public void SetAnimatorMove(bool val)
    {
        animator.SetBool("isMove", val);
    }      
}