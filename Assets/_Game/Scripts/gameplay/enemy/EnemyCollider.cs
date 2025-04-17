using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    [SerializeField] EnemyObject enemyObject;

    public EnemyObject EnemyObject => enemyObject;
}
