using System.Collections.Generic;
using UniRx;
using UnityEngine;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    [Header("other")]
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
    }

    public void Revival()
    {
        hpRx.Value = GameConfig.Instance.maxHpSoldier;
        SetLayerUpperBodyWeight(1);
        SetAnimationDeath(false);
        SetAnimatorMove(false);

        isDeath = false;
    }

    public void InscreaseHp(int val)
    {
        var tempHp = hpRx.Value + val;

        if (tempHp > GameConfig.Instance.maxHpSoldier)
        {
            hpRx.Value = GameConfig.Instance.maxHpSoldier;
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

        if (hpRx.Value <= 0)
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