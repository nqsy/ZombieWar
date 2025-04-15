using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyObject;

public class GameplayManager : SingletonBehaviour<GameplayManager>
{
    public float pixelMargin = 20f;
    Cooldown cdSpawnEnemy;

    Camera mainCamera;

    private void Start()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif

        mainCamera = Camera.main;

        cdSpawnEnemy = new Cooldown(10);
        cdSpawnEnemy.SetRemain(0);
    }

    private void Update()
    {
        cdSpawnEnemy.ReduceCooldown();

        if (cdSpawnEnemy.IsFinishing)
        {
            SpawnEnemy();
            cdSpawnEnemy.Restart();
        }
    }

    void SpawnEnemy()
    {
        EnemyData enemyData = new EnemyData();
        enemyData.maxHp = 100;
        enemyData.speed = 1.5f;
        enemyData.durationAttack = 0.05f;

        for (int i = 0; i < 20; i ++)
        {
            SpawnEnemy(enemyData);
        }
    }

    void SpawnEnemy(EnemyData enemyData)
    {
        var posSpawn = GetEnemyPos();

        EnemyManager.instance.SpawnEnemy(enemyData, posSpawn);
    }

    public Vector3 GetEnemyPos()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        int side = Random.Range(0, 4); 

        Vector3 screenPos = Vector3.zero;

        switch (side)
        {
            case 0: // Top
                screenPos = new Vector3(Random.Range(-pixelMargin, screenWidth + pixelMargin), screenHeight + pixelMargin, 0);
                break;
            case 1: // Bottom
                screenPos = new Vector3(Random.Range(-pixelMargin, screenWidth + pixelMargin), -pixelMargin, 0);
                break;
            case 2: // Left
                screenPos = new Vector3(-pixelMargin, Random.Range(-pixelMargin, screenHeight + pixelMargin), 0);
                break;
            case 3: // Right
                screenPos = new Vector3(screenWidth + pixelMargin, Random.Range(-pixelMargin, screenHeight + pixelMargin), 0);
                break;
        }

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.y = 0;

        return worldPos;
    }
}
