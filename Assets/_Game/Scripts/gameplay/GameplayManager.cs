using Cysharp.Threading.Tasks;
using UnityEngine;
using static EnemyObject;

public class GameplayManager : SingletonBehaviour<GameplayManager>
{
    public static int mapId;
    public float pixelMargin = 20f;
    Cooldown cdSpawnEnemy;

    Camera mainCamera;
    [SerializeField] Transform parentMap;

    public Cooldown cdPlayTime;
    public bool isPauseGame = false;

    private void Start()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
        LoadMap();

        mainCamera = Camera.main;

        cdSpawnEnemy = new Cooldown(GameConfig.instance.durationSpawnEnemy);
        cdPlayTime = new Cooldown(GameConfig.instance.timePlayGame);
        cdSpawnEnemy.SetRemain(0);
    }

    private void Update()
    {
        if (isPauseGame) return;

        cdSpawnEnemy.ReduceCooldown();
        cdPlayTime.ReduceCooldown();

        if (cdSpawnEnemy.IsFinishing)
        {
            SpawnEnemy();
            cdSpawnEnemy.Restart();
        }

        if(cdPlayTime.IsFinishing)
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

        cdSpawnEnemy.Restart();
        cdPlayTime.Restart();
        cdSpawnEnemy.SetRemain(0);

        isPauseGame = false;
    }

    private void LoadMap()
    {
        var map = GameConfig.instance.GetMap(mapId);
        Instantiate(map, parentMap);
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < GameConfig.instance.totalSpawnEnemy; i++)
        {
            SpawnEnemy(GameConfig.instance.enemyData);
        }
    }

    void SpawnEnemy(EnemyData enemyData)
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
