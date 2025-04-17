using System.Collections.Generic;
using System.Net.WebSockets;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.ParticleSystem;

public class EnemyObject : MonoBehaviour
{
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

    public void BeAttack(float dmg, Vector3 impactPos)
    {
        hp -= dmg;
        SpawnBlood(impactPos);

        if (hp < 0)
        {
            EnemyManager.instance.DespawnEnemy(this);
        }
    }

    void SpawnBlood(Vector3 pos)
    {
        EmitParams emitParams = new EmitParams();
        emitParams.position = pos;

        foreach(var particle in particles)
        {
            particle.Emit(emitParams, 1);
        }
    }    

    //for trigger anim
    public void OnTriggerAttack()
    {
        Attack();
    }    
}
