using System.Collections.Generic;
using UnityEngine;
using static EnemyObject;
using static Weapon;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Soldier")]
    public float speedSoldier;
    public int maxHpSoldier;
    public float rotationSpeedSoldier;
    public float maxX;
    public float maxZ;
    public int healHp;

    [Header("Level Map")]
    public List<LevelMap> levelMaps;

    [Header("weapon")]
    public List<WeaponData> weapons;

    [Header("Other")]
    public float durationBomb;
    public float dmgBomb;
    public float timePlayGame;

    public WeaponData GetWeaponData(EWeaponType weaponType)
    {
        return weapons.Find(e => e.weaponType == weaponType);
    }

    public LevelMap GetLevelMap(int mapId)
    {
        return levelMaps[mapId - 1];    
    }

    public static GameConfig instance => Resources.Load<GameConfig>("GameConfig");
}
