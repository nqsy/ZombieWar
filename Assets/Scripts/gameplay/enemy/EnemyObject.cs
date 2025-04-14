using UnityEngine;
using UniRx;

public class EnemyObject : MonoBehaviour
{
    public class EnemyData
    {
        public float maxHp;
    }

    [SerializeField] float speed = 0.05f;
    [SerializeField] ReactiveProperty<float> hpRx = new ReactiveProperty<float>();

    float maxHp;
    float hp { get => hpRx.Value; set => hpRx.Value = value; }

    public void OnSpawn(EnemyData enemyData)
    {
        this.maxHp = enemyData.maxHp;
        hpRx.Value = maxHp;
    }

    private void Update()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        var soldier = SoldierObject.instance;

        transform.LookAt(soldier.transform);

        var normalized = (soldier.transform.position - transform.position).normalized;
        transform.position += normalized * speed;
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
