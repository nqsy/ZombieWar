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
    }

    [SerializeField] NavMeshAgent nav;
    [SerializeField] Animator anim;

    [SerializeField] List<ParticleSystem> particles;

    [Header("for debug")]
    [SerializeField] ReactiveProperty<float> hpRx = new ReactiveProperty<float>();

    [SerializeField] float dis;

    float maxHp;
    float hp { get => hpRx.Value; set => hpRx.Value = value; }
    public bool isAlive => hp > 0;

    public void OnSpawn(EnemyData enemyData)
    {
        this.maxHp = enemyData.maxHp;
        hpRx.Value = maxHp;

        //nav.updateRotation = false;
        nav.updateUpAxis = false;
        nav.speed = enemyData.speed;
    }

    private void Update()
    {
        dis = Vector3.Distance(SoldierObject.instance.transform.position, transform.position);

        if (dis < 1)
        {
            nav.enabled = false;
            anim.SetBool("isAttack", true);
        }
        else
        {
            anim.SetBool("isAttack", false);

            UpdateMovement();
        }
    }

    void UpdateMovement()
    {
        var soldier = SoldierObject.instance;

        nav.enabled = true;
        nav.SetDestination(soldier.transform.position);
    }

    void Attack()
    {
        SoldierObject.instance.BeAttack(10);
    }

    public void BeAttack(float dmg)
    {
        hp -= dmg;
        SpawnBlood();

        if (hp < 0)
        {
            EnemyManager.instance.DespawnEnemy(this);
        }
    }

    void SpawnBlood()
    {
        foreach (var particle in particles)
        {
            particle.Emit(5);
        }
    }

    //for trigger anim
    public void OnTriggerAttack()
    {
        Attack();
    }
}
