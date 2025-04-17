using System.Collections.Generic;
using UnityEngine;

public class SoldierDetect : MonoBehaviour
{
    public List<EnemyObject> enemyObjects = new List<EnemyObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstTag.ENEMY))
        {
            var enemy = other.GetComponent<EnemyCollider>().EnemyObject;
            enemyObjects.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ConstTag.ENEMY))
        {
            var enemy = other.GetComponent<EnemyCollider>().EnemyObject;
            enemyObjects.Remove(enemy);
        }
    }
}
