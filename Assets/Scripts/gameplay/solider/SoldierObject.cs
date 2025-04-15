using UnityEngine;

public partial class SoldierObject : SingletonBehaviour<SoldierObject>
{
    [SerializeField] SoldierDetect soldierDetect;

    private void Start()
    {
        InitAttack();
    }

    private void Update()
    {
        UpdateAttack();
    }

    public void SpawnBomb()
    {
        OtherItemManager.instance.SpawnBomb(transform.position);
    }
}
