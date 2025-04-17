using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class EnemyObject : MonoBehaviour
{
    [System.Serializable]
    public class EnemyData
    {
        public float maxHp;
        public float speed;
        public float rangeAllowAttack;
        public float durationDissolve;
    }

    [SerializeField] NavMeshAgent nav;
    [SerializeField] Animator anim;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;

    [SerializeField] List<ParticleSystem> particleBloods;

    [Header("for debug")]
    [SerializeField] ReactiveProperty<float> hpRx = new ReactiveProperty<float>();

    [SerializeField] float dis;

    float maxHp;
    float hp { get => hpRx.Value; set => hpRx.Value = value; }
    public bool isAlive => hp > 0;
    EnemyData enemyData;

    bool isActiveDissolve = false;
    Cooldown cdDissolve;

    public void OnSpawn(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        this.maxHp = enemyData.maxHp;
        hpRx.Value = maxHp;

        //nav.updateRotation = false;
        nav.updateUpAxis = false;
        nav.speed = enemyData.speed;

        SetDissolveAmount(0);
        cdDissolve = new Cooldown(enemyData.durationDissolve);
        isActiveDissolve = false;

        SetAnimationDeath(0);
    }

    private void Update()
    {
        UpdateMovement();
        UpdateDissolve();
    }

    void UpdateMovement()
    {
        if (!isAlive)
        {
            return;
        }

        dis = Vector3.Distance(SoldierObject.instance.transform.position, transform.position);

        if (dis < enemyData.rangeAllowAttack)
        {
            nav.enabled = false;
            anim.SetBool("isAttack", true);
        }
        else
        {
            anim.SetBool("isAttack", false);

            var soldier = SoldierObject.instance;

            nav.enabled = true;
            nav.SetDestination(soldier.transform.position);
        }
    }

    void Attack()
    {
        SoldierObject.instance.BeAttack(10);
    }

    public void BeAttack(float dmg)
    {
        hp -= dmg;
        ActiveBlood();

        if (hp < 0)
        {
            Death();
        }
    }

    void ActiveBlood()
    {
        foreach (var particle in particleBloods)
        {
            particle.Emit(5);
        }
    }

    //for trigger anim
    public void OnTriggerAttack()
    {
        Attack();
    }

    #region death
    void Death()
    {
        nav.enabled = false;

        int random = Random.Range(1, 6);
        SetAnimationDeath(random);
    }

    void UpdateDissolve()
    {
        if (!isActiveDissolve)
            return;

        cdDissolve.ReduceCooldown();
        var procress = cdDissolve.Process;
        SetDissolveAmount(procress);

        if (cdDissolve.IsFinishing)
        {
            Despawn();
        }
    }

    void SetDissolveAmount(float val)
    {
        skinnedMeshRenderer.material.SetFloat("_DissolveAmount", val);
    }

    /// <summary>
    /// val = 0 => Alive
    /// </summary>
    void SetAnimationDeath(int val)
    {
        anim.SetInteger("death", val);
    }

    //for trigger anim
    public void OnTriggerStartDissolve()
    {
        isActiveDissolve = true;
        cdDissolve.Restart();
    }
    #endregion

    public void Despawn()
    {

        EnemyManager.instance.DespawnEnemy(this);
    }
}
