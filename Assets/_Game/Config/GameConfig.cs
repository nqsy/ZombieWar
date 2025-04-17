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

    [Header("enemy")]
    public EnemyData enemyData;

    [Header("weapon")]
    public List<WeaponData> weapons;

    [Header("Other")]
    public float durationBomb;
    public float dmgBomb;

    public WeaponData GetWeaponData(EWeaponType weaponType)
    {
        return weapons.Find(e => e.weaponType == weaponType);
    }

    public static GameConfig instance => Resources.Load<GameConfig>("GameConfig");
}
