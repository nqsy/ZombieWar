using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
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
    public bool isDeath = false;

    private void Start()
    {
        InitWeapon();
        Revival();
    }

    private void Update()
    {
        if (isDeath) return;

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

    public void Revival()
    {
        hpRx.Value = GameConfig.instance.maxHpSoldier;
        SetLayerUpperBodyWeight(1);
        SetAnimationDeath(false);
        SetAnimatorMove(false);

        isDeath = false;
    }    

    public void InscreaseHp(int val)
    {
        var tempHp = hpRx.Value + val;

        if (tempHp > GameConfig.instance.maxHpSoldier)
        {
            hpRx.Value = GameConfig.instance.maxHpSoldier;
        }
        else
        {
            hpRx.Value = tempHp;
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

        if(hpRx.Value <= 0)
        {
            StartDeath();
        }    
    }

    void ActiveBlood()
    {
        foreach (var particle in particleBloods)
        {
            particle.Emit(5);
        }
    }

    void StartDeath()
    {
        SetLayerUpperBodyWeight(0);
        SetAnimationDeath(true);

        isDeath = true;
    }

    #region animation
    void SetLayerUpperBodyWeight(float val)
    {
        animator.SetLayerWeight(1, val);
    }    

    void SetAnimationDeath(bool val)
    {
        animator.SetBool("isDeath", val);
    }    

    void SetAnimatorFire(bool val)
    {
        animator.SetBool("isFire", val);
    }    

    void SetAnimatorMove(bool val)
    {
        animator.SetBool("isMove", val);
    }
    #endregion
}