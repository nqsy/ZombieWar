using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class EnemyObject : MonoBehaviour
{
    public class EnemyData
    {
        public float maxHp;
        public float speed;
        public float durationAttack;
    }

    [SerializeField] NavMeshAgent nav;
    [SerializeField] ReactiveProperty<float> hpRx = new ReactiveProperty<float>();

    [SerializeField] float dis;

    Cooldown cdAttack;

    float maxHp;
    float hp { get => hpRx.Value; set => hpRx.Value = value; }

    public void OnSpawn(EnemyData enemyData)
    {
        this.maxHp = enemyData.maxHp;
        hpRx.Value = maxHp;

        //nav.updateRotation = false;
        nav.updateUpAxis = false;
        nav.speed = enemyData.speed;

        cdAttack = new Cooldown(enemyData.durationAttack);
        cdAttack.SetRemain(0);
    }

    private void Update()
    {
        dis = Vector2.SqrMagnitude(SoldierObject.instance.transform.position - transform.position);

        if (dis < 1)
        {
            nav.enabled = false;
            
            if(cdAttack.IsFinishing)
            {
                Attack();
                cdAttack.Restart();
            }
            else
            {
                cdAttack.ReduceCooldown();
            }
        }
        else
        {
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

        if (hp < 0)
        {
            EnemyManager.instance.DespawnEnemy(this);
        }
    }
}
