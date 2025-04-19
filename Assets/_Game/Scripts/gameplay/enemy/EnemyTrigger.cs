using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] EnemyObject enemyObject;

    //for trigger anim
    public void OnTriggerAttack()
    {
        enemyObject.StartAttack();
    }

    //for trigger anim
    public void OnTriggerStartDissolve()
    {
        enemyObject.StartDissolve();
    }
}
