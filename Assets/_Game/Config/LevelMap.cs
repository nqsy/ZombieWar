using UnityEngine;
using static EnemyObject;

[System.Serializable]
public class LevelMap
{
    [System.Serializable]
    public class LevelMapDetail
    {
        public float durationSpawnNormalEnemy;
        public float descreseDurationSpawn;
        public float minDurationSpawn;

        public int amountSpawnNormalEnemy;
        public int íncreaseAmount;
        public int maxAmountEnemy;

        public float GetDurationCurrent(int wave)
        {
            var ret = durationSpawnNormalEnemy - descreseDurationSpawn * (wave - 1);
            if(ret < minDurationSpawn)
                ret = minDurationSpawn;

            return ret;
        }

        public int GetAmountSpawn(int wave)
        {
            var ret = amountSpawnNormalEnemy + íncreaseAmount * (wave - 1);
            if (ret > maxAmountEnemy)
                ret = maxAmountEnemy;

            return ret;
        }
    }

    public GameObject map;

    public EnemyData normalEnemyData;
    public EnemyData bigEnemyData;

    public LevelMapDetail normalEnemyDetail;
    public LevelMapDetail bigEnemyDetail;

    public int maxEnemyOnMap;
}
