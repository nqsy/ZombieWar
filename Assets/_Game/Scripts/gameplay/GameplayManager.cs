using Cysharp.Threading.Tasks;
using UnityEngine;
using static EnemyObject;

public class GameplayManager : SingletonBehaviour<GameplayManager>
{
    public static int mapId;
    public float pixelMargin = 20f;
    Cooldown cdSpawnNormalEnemy;
    Cooldown cdSpawnBigEnemy;

    Camera mainCamera;
    [SerializeField] Transform parentMap;

    public Cooldown cdPlayTime;
    public bool isPauseGame = false;

    public LevelMap levelMap;
    int wave = 1;

    private void Start()
    {
        levelMap = GameConfig.instance.GetLevelMap(mapId);

        LoadMap();

        mainCamera = Camera.main;

        cdSpawnNormalEnemy = new Cooldown(levelMap.normalEnemyDetail.GetDurationCurrent(wave));
        cdSpawnBigEnemy = new Cooldown(levelMap.bigEnemyDetail.GetDurationCurrent(wave));
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

        if (cdSpawnNormalEnemy.IsFinishing)
        {
            SpawnNormalEnemy();
            cdSpawnNormalEnemy.Restart();
        }

        if (cdSpawnBigEnemy.IsFinishing)
        {
            SpawnBigEnemy();
            cdSpawnBigEnemy.Restart();
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

        wave = 1;
        isPauseGame = false;
    }

    private void LoadMap()
    {
        Instantiate(levelMap.map, parentMap);
    }

    void SpawnBigEnemy()
    {
        for (int i = 0; i < levelMap.bigEnemyDetail.GetAmountSpawn(wave); i++)
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
        for (int i = 0; i < levelMap.normalEnemyDetail.GetAmountSpawn(wave); i++)
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
