using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    [SerializeField] float speed = 0.05f;

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
}
