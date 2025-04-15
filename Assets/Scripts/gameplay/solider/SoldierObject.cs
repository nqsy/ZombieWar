using UnityEngine;
using UniRx;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    [SerializeField] SoldierDetect soldierDetect;

    public float maxHp = 1000;
    public ReactiveProperty<float> hpRx = new ReactiveProperty<float>();

    private void Start()
    {
        InitAttack();

        hpRx.Value = maxHp;
    }

    private void Update()
    {
        UpdateAttack();
    }

    public void SpawnBomb()
    {
        OtherItemManager.instance.SpawnBomb(transform.position);
    }

    public void BeAttack(float dmg)
    {
        hpRx.Value -= dmg;
    }
}
