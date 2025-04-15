using UnityEngine;
using UniRx;
using UnityEngine.AI;

public class EnemyObject : MonoBehaviour
{
    public class EnemyData
    {
        public float maxHp;
        public float speed;
    }

    [SerializeField] NavMeshAgent nav;
    [SerializeField] ReactiveProperty<float> hpRx = new ReactiveProperty<float>();

    float maxHp;
    float hp { get => hpRx.Value; set => hpRx.Value = value; }

    public void OnSpawn(EnemyData enemyData)
    {
        this.maxHp = enemyData.maxHp;
        hpRx.Value = maxHp;

        nav.updateRotation = false;
        nav.updateUpAxis = false;
        nav.speed = enemyData.speed;
    }

    private void Update()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        var soldier = SoldierObject.instance;

        //transform.LookAt(soldier.transform);

        //var normalized = (soldier.transform.position - transform.position).normalized;
        //transform.position += normalized * speed;

        nav.enabled = true;
        nav.SetDestination(soldier.transform.position);
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
