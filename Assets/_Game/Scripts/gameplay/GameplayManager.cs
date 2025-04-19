using UnityEngine;
using static EnemyObject;

public class GameplayManager : SingletonBehaviour<GameplayManager>
{
    const float MARGIN = 0.2f;

    public static int mapId;
    Cooldown cdSpawnNormalEnemy;
    Cooldown cdSpawnBigEnemy;

    Camera mainCamera;
    [SerializeField] Transform parentMap;

    public Cooldown cdPlayTime;
    public bool isPauseGame = false;

    public LevelMap levelMap;
    int normalWave = 1;
    int bigWave = 1;

    private void Start()
    {
        levelMap = GameConfig.instance.GetLevelMap(mapId);

        LoadMap();

        mainCamera = Camera.main;

        cdSpawnNormalEnemy = new Cooldown(levelMap.normalEnemyDetail.GetDurationCurrent(normalWave));
        cdSpawnBigEnemy = new Cooldown(levelMap.bigEnemyDetail.GetDurationCurrent(bigWave));
        cdPlayTime = new Cooldown(GameConfig.instance.timePlayGame);
        cdSpawnNormalEnemy.SetRemain(0);
        cdSpawnBigEnemy.SetRemain(0);
    }

    private void Update()
    {
        if (isPauseGame) return;

        cdSpawnNormalEnemy.ReduceCooldown();
        cdSpawnBigEnemy.ReduceCooldown();
        cdPlayTime.ReduceCooldown();

        if (cdSpawnNormalEnemy.IsFinishing && cdSpawnNormalEnemy.Duration > 0)
        {
            SpawnNormalEnemy();
            cdSpawnNormalEnemy.Restart();
            normalWave++;
        }

        if (cdSpawnBigEnemy.IsFinishing && cdSpawnBigEnemy.Duration > 0)
        {
            SpawnBigEnemy();
            cdSpawnBigEnemy.Restart();
            bigWave++;
        }

        if (cdPlayTime.IsFinishing)
        {
            UIGameplay.instance.OpenVictoryPopup();
        }
    }

    public void RetryGame()
    {
        EnemyManager.instance.DespawnAllEnemy();
        BulletManager.instance.DespawnAllBullet();
        ExploreManager.instance.DespawnAllExplore();
        OtherItemManager.instance.DespawnAllOther();
        SoldierObject.instance.Revival();

        cdSpawnNormalEnemy.Restart();
        cdSpawnBigEnemy.Restart();
        cdSpawnNormalEnemy.SetRemain(0);
        cdSpawnBigEnemy.SetRemain(0);

        cdPlayTime.Restart();

        normalWave = 1;
        bigWave = 1;
        isPauseGame = false;
    }

    private void LoadMap()
    {
        Instantiate(levelMap.map, parentMap);
    }

    void SpawnBigEnemy()
    {
        for (int i = 0; i < levelMap.bigEnemyDetail.GetAmountSpawn(bigWave); i++)
        {
            SpawnBigEnemy(levelMap.bigEnemyData);
        }
    }

    void SpawnBigEnemy(EnemyData enemyData)
    {
        if (!EnemyManager.instance.IsCanSpawnEnemy())
            return;

        var posSpawn = GetEnemyPos();

        EnemyManager.instance.SpawnBigEnemy(enemyData, posSpawn);
    }

    void SpawnNormalEnemy()
    {
        for (int i = 0; i < levelMap.normalEnemyDetail.GetAmountSpawn(normalWave); i++)
        {
            SpawnNormalEnemy(levelMap.normalEnemyData);
        }
    }

    void SpawnNormalEnemy(EnemyData enemyData)
    {
        if (!EnemyManager.instance.IsCanSpawnEnemy())
            return;

        var posSpawn = GetEnemyPos();

        EnemyManager.instance.SpawnNormalEnemy(enemyData, posSpawn);
    }

    public Vector3 GetEnemyPos()
    {
        int side = Random.Range(0, 4);

        Vector3 worldPos = Vector3.zero;

        switch (side)
        {
            case 0: // Top
                worldPos = mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 1f + MARGIN, mainCamera.nearClipPlane));
                break;
            case 1: // Bottom
                worldPos = mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 0f - MARGIN, mainCamera.nearClipPlane));
                break;
            case 2: // Left
                worldPos = mainCamera.ViewportToWorldPoint(new Vector3(0f - MARGIN, Random.Range(0f, 1f), mainCamera.nearClipPlane));
                break;
            case 3: // Right
                worldPos = mainCamera.ViewportToWorldPoint(new Vector3(1f + MARGIN, Random.Range(0f, 1f), mainCamera.nearClipPlane));
                break;
        }

        worldPos.y = 0;

        return worldPos;
    }
}
